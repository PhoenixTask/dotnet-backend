using System.Net.Mail;

namespace Application.Abstractions.Emails;
public interface IEmailService
{
    Task SendEmailAsync(string emailTo,string subject,string body);
}
