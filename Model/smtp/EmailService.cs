using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Portfolio.Model.Smtp;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string subject, string htmlMessage, string? to = null)
    {
        string displayName = _smtpSettings.MyName;
        if (string.IsNullOrEmpty(to))
        {
            to = _smtpSettings.MyAddr;
            displayName = _smtpSettings.MyMessageDisplayName;
        }

        using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
        {
            smtpClient.EnableSsl = _smtpSettings.EnableSsl;
            smtpClient.UseDefaultCredentials = _smtpSettings.UseDefaultCredentials;
            smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, displayName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
