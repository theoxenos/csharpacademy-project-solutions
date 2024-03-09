namespace SportsResultsNotifier.Utils;

public class MailServerSettings
{
    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool SSL { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}