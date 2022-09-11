using System.Threading.Tasks;
using Inviscan.Models;
using Inviscan.Sync.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Inviscan.Sync.Commands
{
    public sealed class SyncCollection : ICommand
    {
        #region Fields
        private readonly ILogger<SyncCollection> logger;
        private readonly IInventoryDataService   inventoryDataService;
        private readonly ICollectionLogService   collectionLogService;
        private readonly IConfiguration          configuration;
        #endregion

        public SyncCollection(ILogger<SyncCollection> logger,
                              IInventoryDataService inventoryDataService,
                              ICollectionLogService collectionLogService,
                              IConfiguration configuration)
        {
            this.logger               = logger;
            this.inventoryDataService = inventoryDataService;
            this.collectionLogService = collectionLogService;
            this.configuration        = configuration;
        }
        
        public async Task Execute()
        {
            var sheetsConfiguration = SheetsConfiguration.GetFromConfiguration(configuration);
            
            foreach (var inventoryConfiguration in InventoryConfiguration.GetFromConfiguration(configuration))
            {
                foreach (var inventoryPageConfiguration in await inventoryDataService.GetPageConfigurations(sheetsConfiguration, inventoryConfiguration))
                {
                    var page = await inventoryDataService.GetPage(sheetsConfiguration, inventoryConfiguration, inventoryPageConfiguration);
                    
                    logger.LogInformation("Syncing page...");
                }
            }
        }
    }
}