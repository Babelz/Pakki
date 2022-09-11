using System;
using System.Collections.Generic;
using Ardalis.SmartEnum;

namespace Inviscan.Models
{
    public sealed class Package : SmartEnum<Package>
    {
        #region Public fields
        public static readonly Package C = new Package(nameof(C), 0);
        public static readonly Package B = new Package(nameof(B), 1);
        public static readonly Package L = new Package(nameof(L), 2);
        public static readonly Package I = new Package(nameof(I), 3);
        public static readonly Package S = new Package(nameof(S), 4);
        #endregion

        #region Private fields
        private static readonly Dictionary<Package, string> Descriptions = new Dictionary<Package, string>()
        {
            { C, "CIB, complete in box" }, 
            { B, "Boxed, missing instructions or box internals" }, 
            { L, "Loose, missing manual and box" }, 
            { I, "Loose with manual" },
            { S, "Sealed" },   
        };
        #endregion
        
        private Package(string name, int value) 
            : base(name, value)
        {
        }
        
        public static string GetDescription(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));
            
            if (!Descriptions.TryGetValue(package, out var description))
                throw new ArgumentException($"No description found for package {package.Name}", nameof(package));

            return description;
        }
    }
}