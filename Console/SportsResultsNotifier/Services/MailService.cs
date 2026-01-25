using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SportsResultsNotifier.Utils;

namespace SportsResultsNotifier.Services;

public class MailService(IOptions<MailServerSettings> options)
{
    private readonly MailServerSettings _options = options.Value;

    public void SendMail(string body)
    {
        ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;

        var mail = new MailMessage(_options.From, _options.To)
        {
            IsBodyHtml = true,
            Body = body,
            Subject = _options.Subject
        };

        SmtpClient smtpClient = new(_options.Host)
        {
            Credentials = new NetworkCredential(_options.UserName, _options.Password),
            Port = _options.Port,
            EnableSsl = _options.Ssl
        };

        try
        {
            smtpClient.Send(mail);
            Console.WriteLine("Mail sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send mail: " + ex.Message);
        }
    }
}