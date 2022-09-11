using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Inviscan.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Inviscan.Sync.Services
{
    /// <summary>
    /// Structure that defines details for single inventory page. These details are used for fetching the inventory contents and more detailed information for specific
    /// pages.
    /// </summary>
    public readonly struct InventoryPageConfiguration
    {
        #region Properties
        public string Title
        {
            get;
        }
        
        public Vendor Vendor
        {
            get;
        }

        public ConsoleType Console
        {
            get;
        }

        public Category Category
        {
            get;
        }
        #endregion

        public InventoryPageConfiguration(string title, Vendor vendor, ConsoleType console, Category category)
        {
            Title    = !string.IsNullOrEmpty(title) ? title : throw new ArgumentNullException(nameof(title));
            Vendor   = vendor;
            Console  = console ?? throw new ArgumentNullException(nameof(console));
            Category = category;
        }
    }

    public struct InventoryConfiguration
    {
        #region Properties
        public Guid Id
        {
            get;
            set;
        }

        public string Author
        {
            get;
            set;
        }

        public string SheetId
        {
            get;
            set;
        }
        #endregion

        public static IEnumerable<InventoryConfiguration> GetFromConfiguration(IConfiguration configuration)
            => configuration.GetSection("Inventories").Get<InventoryConfiguration[]>();
    }
    
    /// <summary>
    /// Interface for implementing services that provide information about the retro inventory.
    /// </summary>
    public interface IInventoryDataService
    {
        /// <summary>
        /// Returns all inventory page configurations for given inventory configuration.
        /// </summary>
        Task<IEnumerable<InventoryPageConfiguration>> GetPageConfigurations(SheetsConfiguration sheetsConfiguration, InventoryConfiguration inventoryConfiguration);

        /// <summary>
        /// Returns single inventory page contents that match the given page configuration from collection that matches the given inventory configuration. 
        /// </summary>
        Task<InventoryPage> GetPage(SheetsConfiguration sheetsConfiguration, InventoryConfiguration inventoryConfiguration, InventoryPageConfiguration pageConfiguration);
    }

    public struct SheetsConfiguration
    {   
        #region Properties
        public string ApplicationName
        {
            get;
            set;
        }

        public string Secrets
        {
            get;
            set;
        }
        #endregion

        public static SheetsConfiguration GetFromConfiguration(IConfiguration configuration)
            => configuration.GetSection("GoogleSheets").Get<SheetsConfiguration>();
    }

    /// <summary>
    /// Static utility class that contains mappings for working the the inventory sheets.
    /// </summary>
    public static class SheetDataMappings
    {
        public static class Configuration
        {
            #region Constant fields
            public const string Name = "Configuration";
            #endregion
            
            public static class Locations
            {
                #region Constant fields
                public const byte Header   = 0;
                public const byte Vendor   = 1;
                public const byte Console  = 2;
                public const byte Category = 3;
                #endregion
            }
            
            public static class Regions
            {
                #region Constant fields
                public const string Start = "A4";
                public const string End   = "B7";
                #endregion
            }
        }
        
        public static class Items
        {
            public static class Regions
            {
                #region Constant fields
                public const string Start = "A10";
                #endregion
            }
        }
    }
    
    public class InventoryDataService : IInventoryDataService
    {   
        #region Fields
        private readonly ILogger<InventoryDataService> logger;
        #endregion

        public InventoryDataService(ILogger<InventoryDataService> logger)
            => this.logger = logger;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SheetsService CreateGoogleSheetsClient(SheetsConfiguration sheetsConfiguration)
        {
            using var fs = new FileStream(sheetsConfiguration.Secrets, FileMode.Open);

            var credential = GoogleCredential.FromStream(fs).CreateScoped(SheetsService.Scope.Spreadsheets);
            
            return new SheetsService(new BaseClientService.Initializer()     
            {
                HttpClientInitializer = credential,
                ApplicationName       = sheetsConfiguration.ApplicationName
            });
        }
        
        public async Task<IEnumerable<InventoryPageConfiguration>> GetPageConfigurations(SheetsConfiguration sheetsConfiguration, 
                                                                                         InventoryConfiguration inventoryConfiguration)
        {
            logger.LogInformation("Fetching inventory page configuration for inventory {@inventory}", inventoryConfiguration);

            // Get sheet details from Google sheets.
            var results  = new List<InventoryPageConfiguration>();
            var client   = CreateGoogleSheetsClient(sheetsConfiguration);
            var response = await client.Spreadsheets.Get(inventoryConfiguration.SheetId).ExecuteAsync();

            // Process each sheet.
            foreach (var sheet in response.Sheets)
            {
                // Get configuration region data from the sheet.
                var configurationData = await client.Spreadsheets.Values.Get(
                        inventoryConfiguration.SheetId, 
                        $"{sheet.Properties.Title}!{SheetDataMappings.Configuration.Regions.Start}:{SheetDataMappings.Configuration.Regions.End}")
                    .ExecuteAsync();
                
                // Do not parse pages that do not contain page configuration section. 
                if (configurationData.Values[SheetDataMappings.Configuration.Locations.Header].FirstOrDefault()?.ToString() != SheetDataMappings.Configuration.Name)
                {
                    logger.LogInformation($"Sheet {sheet.Properties.Title} does not contain page configuration section, skipping parsing...");
                    
                    continue;
                }

                // Parse page configuration values from the sheet. Omit parsing if the page configuration is invalid.
                if (!Enum.TryParse<Vendor>(configurationData.Values[SheetDataMappings.Configuration.Locations.Vendor].Last().ToString(), out var vendor))
                {
                    logger.LogWarning($"Invalid {nameof(Vendor)} value in sheet {sheet.Properties.Title}, can't load page configuration");
                    
                    continue;
                }
                
                if (!Enum.TryParse<Category>(configurationData.Values[SheetDataMappings.Configuration.Locations.Category].Last().ToString(), out var category))
                {
                    logger.LogWarning($"Invalid {nameof(Category)} value in sheet {sheet.Properties.Title}, can't load page configuration");
                    
                    continue;
                }
                
                if (!ConsoleType.TryFromName(configurationData.Values[SheetDataMappings.Configuration.Locations.Console].Last().ToString(), out var console))
                {
                    logger.LogWarning($"Invalid {nameof(ConsoleType)} value in sheet {sheet.Properties.Title}, can't load page configuration");
                    
                    continue;
                }

                // At this point parsing of the page configuration should be ok.
                results.Add(new InventoryPageConfiguration(sheet.Properties.Title, vendor, console, category));
            }
            
            logger.LogInformation($"Found total {results.Count} configured pages from the sheet");

            return results;
        }

        public async Task<InventoryPage> GetPage(SheetsConfiguration sheetsConfiguration,
                                                 InventoryConfiguration inventoryConfiguration,
                                                 InventoryPageConfiguration pageConfiguration)
        {
            logger.LogInformation("Fetching inventory page for inventory {@inventory} using following page configuration {@configuration}", 
                                  inventoryConfiguration, 
                                  pageConfiguration);

            return await Task.FromResult<InventoryPage>(null);
        }
    }
}