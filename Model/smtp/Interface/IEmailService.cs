namespace Portfolio.Model.Smtp;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlMessage, string from = "Marc Proux");
}