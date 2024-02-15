namespace Portfolio.Model.Smtp;

public interface IEmailService
{
    Task SendEmailAsync(string subject, string htmlMessage, string? to = null);
}