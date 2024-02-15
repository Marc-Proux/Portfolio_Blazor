using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Portfolio.Data;
using Portfolio.Data.Models.Interface;
using Portfolio.Model.Smtp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();

builder.Services.AddOptions<FormationApiJsonDirectAccessSetting>().Configure(options =>
{
    options.DataPath = @"Data/";
    options.FormationsFolder = "Formations";
    options.ExperiencesFolder = "Experiences";
    options.ProjectsFolder = "Projects";
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

