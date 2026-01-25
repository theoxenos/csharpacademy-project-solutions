using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using SportsResultsNotifier.Utils;

namespace SportsResultsNotifier.Services;

public class MailService(IOptions<MailServerSettings> options)
{
    private readonly MailServerSettings _options = options.Value;

    public void SendMail(string body)
    {
        using MimeMessage mail = CreateMimeMessage(body);

        using SmtpClient smtpClient = new();
        smtpClient.Connect(_options.Host, _options.Port, _options.Ssl);
        smtpClient.Authenticate(_options.UserName, _options.Password);

        try
        {
            smtpClient.Send(mail);
            Console.WriteLine("Mail sent successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send mail: {ex.Message}");
        }
        finally
        {
            smtpClient.Disconnect(true);
        }
    }

    private MimeMessage CreateMimeMessage(string body)
    {
        var mimeMessage = new MimeMessage
        {
            Body = new TextPart(TextFormat.Html) { Text = body },
            Subject = _options.Subject
        };

        mimeMessage.From.Add(MailboxAddress.Parse(_options.From));
        mimeMessage.To.Add(MailboxAddress.Parse(_options.To));

        return mimeMessage;
    }
}