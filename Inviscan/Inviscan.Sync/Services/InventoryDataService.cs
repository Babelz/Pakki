using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inviscan.Models;
using Microsoft.Extensions.Logging;

namespace Inviscan.Sync.Services
{
    /// <summary>
    /// Structure that contains the item condition values.
    /// </summary>
    public struct ItemGrade
    {
        #region Properties
        /// <summary>
        /// Gets the condition of the box.
        /// </summary>
        public float Box
        {
            get;
        }

        /// <summary>
        /// Gets the condition of the manual.
        /// </summary>
        public float Manual
        {
            get;
        }

        /// <summary>
        /// Gets the condition of the game.
        /// </summary>
        public float Game
        {
            get;
        }

        /// <summary>
        /// Gets the average grade that is the average of <see cref="Box"/>, <see cref="Manual"/> and <see cref="Game"/> values.
        /// </summary>
        public float Average
        {
            get;
        }

        /// <summary>
        /// Gets the user defined condition average condition of the item. 
        /// </summary>
        public float Condition
        {
            get;
        }
        #endregion

        public ItemGrade(float box, float manual, float game, float condition)
        {
            Box       = box < 0.0f ? throw new ArgumentOutOfRangeException(nameof(box), "Expecting positive value") : box;
            Manual    = manual < 0.0f ? throw new ArgumentOutOfRangeException(nameof(manual), "Expecting positive value") : manual;
            Game      = game < 0.0f ? throw new ArgumentOutOfRangeException(nameof(game), "Expecting positive value") : game;
            Condition = box < 0.0f ? throw new ArgumentOutOfRangeException(nameof(condition), "Expecting positive value") : condition;

            Average = box + manual + game;

            if (Average < 0.0f)
                throw new InvalidOperationException("Average can't be below zero");

            if (Average > 0.0f)
                Average /= 3;
        }
    }

    /// <summary>
    /// Structure that represents retro inventory item.
    /// </summary>
    public sealed class InventoryItem
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

        public ItemGrade Grade
        {
            get;
        }
        #endregion

        public InventoryItem(string name, string notes, short quantity, Region region, Category category, ConsoleType consoleType, in ItemGrade grade)
        {
            Name     = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            Notes    = notes;
            Quantity = quantity;
            Region   = region;
            Grade    = grade;
        }
    }

    public sealed class InventoryPage : IEnumerable<InventoryItem>
    {
        #region Fields
        private readonly IList<InventoryItem> items;
        #endregion
        
        #region Properties
        public string Name
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
        #endregion

        public InventoryPage(string name, ConsoleType consoleType, Category category, IList<InventoryItem> items)
        {
            Name        = !string.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            ConsoleType = consoleType;
            Category    = category;
            
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IEnumerator<InventoryItem> GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
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