using System.Threading.Tasks;
using Inviscan.Models;
using Inviscan.Sync.Services;
using Microsoft.Extensions.Logging;

namespace Inviscan.Sync.Commands
{
    public sealed class SyncCollection : ICommand
    {
        #region Fields
        private readonly ILogger<SyncCollection> logger;
        private readonly IInventoryDataService   inventoryDataService;
        private readonly ICollectionLogService   collectionLogService;
        #endregion

        public SyncCollection(ILogger<SyncCollection> logger, IInventoryDataService inventoryDataService, ICollectionLogService collectionLogService)
        {
            this.logger               = logger;
            this.inventoryDataService = inventoryDataService;
            this.collectionLogService = collectionLogService;
        }
        
        public async Task Execute()
        {
            foreach (var consoleType in ConsoleType.List)
                await inventoryDataService.GetConsoleInventory(consoleType);
        }
    }
}