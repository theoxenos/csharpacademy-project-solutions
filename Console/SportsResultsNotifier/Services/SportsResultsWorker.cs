using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SportsResultsNotifier.Controllers;

namespace SportsResultsNotifier.Services;

public class SportsResultsWorker(
    ILogger<SportsResultsWorker> logger,
    IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Sports Results Notifier Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);

            try
            {
                using IServiceScope scope = serviceProvider.CreateScope();
                var controller = scope.ServiceProvider.GetRequiredService<MainController>();
                controller.Start();
                logger.LogInformation("Sports results processed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing sports results.");
            }

            // Wait for 24 hours
            logger.LogInformation("Waiting for 24 hours before next execution...");
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        logger.LogInformation("Sports Results Notifier Service is stopping.");
    }
}