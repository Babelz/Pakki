using System;
using System.IO;
using Inviscan.Sync.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Inviscan.Sync
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                           .ConfigureAppConfiguration(builder => builder.SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                                                        .AddJsonFile("appsettings.json", false))
                           .ConfigureServices((context, services) =>
                            {
                                services.AddSingleton<IInventoryDataService, InventoryDataService>();
                                services.AddSingleton<IInventoryDatabaseService, InventoryDatabaseDatabaseService>();
                                services.AddSingleton<IProductDataService, ProductDataService>();
                            });
        }
    }
}