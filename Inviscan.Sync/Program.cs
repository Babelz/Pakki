using System;
using System.IO;
using Inviscan.Sync.Commands;
using Inviscan.Sync.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Inviscan.Sync
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            // Configure Serilog.
            Log.Logger = new LoggerConfiguration().MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                                                  .Enrich.FromLogContext()
                                                  .WriteTo.Console()
                                                  .WriteTo.Debug()
                                                  .CreateLogger();

            // Build the actual application and cook all the dependencies.
            var host = Host.CreateDefaultBuilder(args)
                           .UseSerilog()
                           .ConfigureAppConfiguration(builder => builder.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                                                        .AddJsonFile("appsettings.json", false))
                           .ConfigureServices((context, services) =>
                            {
                                services.AddSingleton<IInventoryDataService, InventoryDataService>();
                                services.AddSingleton<IPriceDataService, PriceDataService>();
                                services.AddSingleton<ICollectionLogService, CollectionLogService>();
                                services.AddSingleton<ICommand, SyncCollection>();
                            }).Build();
            
            // Run the sync command.
            host.Services.GetService<SyncCollection>()!.Execute();
        }
    }
}