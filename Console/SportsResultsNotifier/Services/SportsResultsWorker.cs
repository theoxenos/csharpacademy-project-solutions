using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsResultsNotifier.Controllers;

namespace SportsResultsNotifier.Services;

public class SportsResultsWorker(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Sports Results Notifier Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");

            try
            {
                using IServiceScope scope = serviceProvider.CreateScope();
                var controller = scope.ServiceProvider.GetRequiredService<MainController>();
                controller.Start();
                Console.WriteLine("Sports results processed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while processing sports results. " + ex.Message);
            }

            // Wait for 24 hours
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        Console.WriteLine("Sports Results Notifier Service is stopping.");
    }
}