# Sports Results Notifier

Console app that scrapes daily NBA box scores and emails a formatted HTML summary. It runs as a hosted background worker and repeats every 24 hours.

## Structure

- `Program.cs`: Host setup, dependency injection, configuration, and worker registration.
- `Controllers/MainController.cs`: Orchestrates the scrape and email flow.
- `Services/SportsResultsWorker.cs`: Background worker loop that runs once every 24 hours.
- `Services/ScraperService.cs`: Scrapes https://www.basketball-reference.com/boxscores/ with HtmlAgilityPack and builds match models.
- `Services/MailService.cs`: Sends the HTML email using MailKit.
- `Models/`: Domain models (`GameMatch`, `Team`).
- `Utils/MailServerSettings.cs`: Configuration binding for mail settings.

## Configuration

Set the following configuration values in `appsettings.json` (or via user secrets/environment variables):

- `MailServerSettings:From`
- `MailServerSettings:To`
- `MailServerSettings:Subject`
- `MailServerSettings:Host`
- `MailServerSettings:Port`
- `MailServerSettings:Ssl`
- `MailServerSettings:UserName`
- `MailServerSettings:Password`

Example `appsettings.json`:

```json
{
  "MailServerSettings": {
    "From": "no-reply@example.com",
    "To": "you@example.com",
    "Subject": "Daily Results",
    "Host": "smtp.example.com",
    "Port": 587,
    "Ssl": true,
    "UserName": "smtp-user",
    "Password": "smtp-pass"
  }
}
```

Optional: user secrets are supported in development.

```bash
dotnet user-secrets init

dotnet user-secrets set "MailServerSettings:Host" "smtp.example.com"
```

## Run

```bash
dotnet restore

dotnet run
```

The worker logs a startup message, processes results immediately, then waits 24 hours before the next run.
