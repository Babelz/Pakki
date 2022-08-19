using System;
using System.Threading.Tasks;
using Inviscan.Models;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Inviscan.Sync.Services
{
    /// <summary>
    /// Structure that represents retro inventory item.
    /// </summary>
    public readonly struct InventoryItem
    {
        #region Properties
        public string Name
        {
            get;
        }

        public string Notes
        {
            get;
        }

        public short Quantity
        {
            get;
        }

        public Region Region
        {
            get;
        }

        public Category Category
        {
            get;
        }

        public ConsoleType ConsoleType
        {
            get;
        }

        /// <summary>
        /// Gets the condition of the item. Grade is usually non-zero positive number.
        /// </summary>
        public float Condition
        {
            get;
        }
        #endregion

        public InventoryItem(string name, string notes, short quantity, Region region, Category category, ConsoleType consoleType, float condition)
        {
            Name        = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            Notes       = notes;
            Quantity    = quantity;
            Region      = region;
            Category    = category;
            ConsoleType = consoleType;
            Condition   = condition;
        }
    }

    /// <summary>
    /// Interface for implementing services that provide information about the retro inventory.
    /// </summary>
    public interface IInventoryDataService
    {
        /// <summary>
        /// Returns array containing all inventory rows for given console. This contains items of all categories. 
        /// </summary>
        Task<InventoryItem[]> GetConsoleInventory(ConsoleType console);
    }

    public class InventoryDataService : IInventoryDataService
    {
        #region Fields
        private readonly ILogger<InventoryDataService> logger;
        #endregion

        public InventoryDataService(ILogger<InventoryDataService> logger)
            => this.logger = logger;

        public async Task<InventoryItem[]> GetConsoleInventory(ConsoleType console)
        {
            logger.LogInformation("Scanning inventory changes for console {0}", console);

            return await Task.FromResult(Array.Empty<InventoryItem>());
        }
    }
}