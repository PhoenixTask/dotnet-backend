using System.Net;
using System.Net.Mail;
using Application.Abstractions.Emails;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Emails;
internal class EmailService(
    IOptions<MailSettings> maiLSettingsOptions,
    ILogger<EmailService> logger) : IEmailService
{
    public async Task SendEmailAsync(string emailTo, string subject, string body)
    {
        using var email = new MailMessage
        {
            From = new MailAddress(maiLSettingsOptions.Value.SenderEmail, maiLSettingsOptions.Value.SenderDisplayName),
            Subject = subject,
            Body = body
        };
        email.To.Add(new MailAddress(emailTo));

        using var smtpClient = new SmtpClient();
        smtpClient.EnableSsl = true;
        smtpClient.Host = maiLSettingsOptions.Value.SmtpServer;
        smtpClient.Port = maiLSettingsOptions.Value.SmtpPort;

        var credentials = new NetworkCredential
        {
            UserName = maiLSettingsOptions.Value.SenderEmail,
            Password = maiLSettingsOptions.Value.SmtpPassword
        };
        smtpClient.Credentials = credentials;

        try
        {
            await smtpClient.SendMailAsync(email);
        }
        catch (Exception exception) 
        {
            logger.LogError(exception, "Email service got error");
        }
    }
}
