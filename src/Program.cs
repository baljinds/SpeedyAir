using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SpeedyAir.ly.src.Models;
using SpeedyAir.Services;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var logger = serviceProvider.GetService<ILogger<Program>>();

        try
        {
            var scheduleManager = serviceProvider.GetRequiredService<IScheduleManager>();
            ProcessFlightsAndOrders(scheduleManager, serviceProvider);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during flight or order processing.");
        }
    }

    /// <summary>
    /// Configures services, including logging, flight data, and order services.
    /// </summary>
    private static ServiceProvider ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging(configure => configure.AddConsole())
            .AddSingleton<FlightDataService>()
            .AddSingleton<OrderDataService>()
            .AddSingleton<IScheduleManager, ScheduleManager>(provider =>
                new ScheduleManager(
                    provider.GetRequiredService<FlightDataService>(),
                    provider.GetRequiredService<ILogger<ScheduleManager>>(),
                    configuration["DataPaths:FlightDataPath"]))
            .BuildServiceProvider();
    }

    /// <summary>
    /// Processes flights and orders by printing, scheduling, and displaying the assigned orders.
    /// </summary>
    private static void ProcessFlightsAndOrders(IScheduleManager scheduleManager, ServiceProvider serviceProvider)
    {
        // Print all flights
        scheduleManager.PrintAllFlights();

        // Load orders from JSON file
        var orderService = serviceProvider.GetRequiredService<OrderDataService>();
        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var orders = orderService.LoadOrders(config["DataPaths:OrderDataPath"]);

        // Assign orders to flights
        scheduleManager.ScheduleOrders(orders);

        // Print the orders assigned to flights
        scheduleManager.PrintAssignedOrders();
    }
}
