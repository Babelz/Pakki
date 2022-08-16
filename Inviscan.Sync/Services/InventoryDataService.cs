using System;
using Inviscan.Models;

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

        public InventoryItem(string name, string notes, short quantity, Region region, ConsoleType consoleType, float condition)
        {
            Name      = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            Notes     = notes;
            Quantity  = quantity;
            Region    = region;
            ConsoleType   = consoleType;
            Condition = condition;
        }
    }
    
    /// <summary>
    /// Interface for implementing services that provide information about the retro inventory.
    /// </summary>
    public interface IInventoryDataService
    {
    }
    
    public class InventoryDataService : IInventoryDataService
    {
    }
}