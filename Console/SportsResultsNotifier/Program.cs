using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResultsNotifier.Controllers;
using SportsResultsNotifier.Services;
using SportsResultsNotifier.Utils;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.Services.Configure<MailServerSettings>(builder.Configuration.GetSection(nameof(MailServerSettings)));

builder.Services.AddScoped<MailService>();
builder.Services.AddScoped<ScraperService>();

builder.Services.AddTransient<MainController>();
builder.Services.AddHostedService<SportsResultsWorker>();

var host = builder.Build();

host.Run();