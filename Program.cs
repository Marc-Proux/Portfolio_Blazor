using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Portfolio.Model;
using Portfolio.Model.Data;
using Portfolio.Model.Data.Interface;
using Portfolio.Model.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PortfolioContextConnection") ?? throw new InvalidOperationException("Connection string 'PortfolioContextConnection' not found.");

builder.Services.AddDbContext<PortfolioContext>(options => options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<PortfolioUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<PortfolioContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

// Add session state services
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache(); // Necessary for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddOptions<FormationApiJsonDirectAccessSetting>().Configure(options =>
{
    options.DataPath = @"wwwroot/Data/";
    options.FormationsFolder = "Formations";
    options.ExperiencesFolder = "Experiences";
    options.ProjectsFolder = "Projects";
    options.HistoryFolder = "History";
});
builder.Services.AddScoped<IPortfolioApi, PortfolioApiJsonDirectAccess>();
builder.Services.Configure<SmtpSettings>(smtpSettings =>
{
    smtpSettings.Host = builder.Configuration["SMTP_Host"];
    smtpSettings.Port = int.Parse(builder.Configuration["SMTP_Port"]);
    smtpSettings.Username = builder.Configuration["SMTP_Username"];
    smtpSettings.Password = builder.Configuration["SMTP_ApiKey"];
    smtpSettings.EnableSsl = bool.Parse(builder.Configuration["SMTP_EnableSsl"]);
    smtpSettings.UseDefaultCredentials = bool.Parse(builder.Configuration["SMTP_UseDefaultCredentials"]);
    smtpSettings.MyAddr = builder.Configuration["SMTP_MyAddr"];
    smtpSettings.MyName = builder.Configuration["SMTP_MyName"];
    smtpSettings.MyMessageDisplayName = builder.Configuration["SMTP_MyMessageDisplayName"];
});

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton<VisitCounterService>(new VisitCounterService("wwwroot/Data/visitCount.txt"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//redirect registration to home page
app.MapGet("/Identity/Account/Register", (HttpContext context) =>
{
    context.Response.Redirect("/");
    return Task.CompletedTask;
});

app.Run();

