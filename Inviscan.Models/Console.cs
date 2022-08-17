using System;
using System.Drawing;
using System.Linq;
using Ardalis.SmartEnum;

namespace Inviscan.Models
{
    [Flags]
    public enum ConsoleKind : byte
    {
        None     = 0,
        Home     = (1 << 0),
        Handheld = (1 << 1)
    }
    
    public sealed class ConsoleType : SmartEnum<ConsoleType>
    {
        #region Nintend home consoles
        public static readonly ConsoleType NES  = new ConsoleType(nameof(NES), 0);
        public static readonly ConsoleType SNES = new ConsoleType(nameof(SNES), 1);
        public static readonly ConsoleType N64  = new ConsoleType(nameof(N64), 2);

        public static readonly ConsoleType GC         = new ConsoleType(nameof(GC), 3);
        public static readonly ConsoleType Wii        = new ConsoleType(nameof(Wii), 4);
        public static readonly ConsoleType WiiU       = new ConsoleType(nameof(WiiU), 5);
        public static readonly ConsoleType Switch     = new ConsoleType(nameof(Switch), 6);
        public static readonly ConsoleType SwitchLite = new ConsoleType(nameof(SwitchLite), 7);
        #endregion

        #region Sony home consoles
        public static readonly ConsoleType PS1 = new ConsoleType(nameof(PS1), 8);
        public static readonly ConsoleType PS2 = new ConsoleType(nameof(PS2), 9);
        public static readonly ConsoleType PS3 = new ConsoleType(nameof(PS3), 10);
        public static readonly ConsoleType PS4 = new ConsoleType(nameof(PS4), 11);
        public static readonly ConsoleType PS5 = new ConsoleType(nameof(PS5), 12);
        #endregion
        
        #region Microsoft home consoles
        public static readonly ConsoleType XB    = new ConsoleType(nameof(XB), 13);
        public static readonly ConsoleType XB360 = new ConsoleType(nameof(XB360), 14);
        public static readonly ConsoleType XBO   = new ConsoleType(nameof(XBO), 15);
        public static readonly ConsoleType XBSX  = new ConsoleType(nameof(XBSX), 16);
        #endregion
        
        #region Sony handheld consoles
        public static readonly ConsoleType PSP  = new ConsoleType(nameof(PSP), 17);
        public static readonly ConsoleType Vita = new ConsoleType(nameof(Vita), 18);
        #endregion
        
        #region Nintendo handheld consoles
        public static readonly ConsoleType GB     = new ConsoleType(nameof(GB), 19);
        public static readonly ConsoleType GBC    = new ConsoleType(nameof(GBC), 20);
        public static readonly ConsoleType GBA    = new ConsoleType(nameof(GBA), 21);
        public static readonly ConsoleType GBASP  = new ConsoleType(nameof(GBASP), 23);
        public static readonly ConsoleType DS     = new ConsoleType(nameof(DS), 24);
        public static readonly ConsoleType DSLite = new ConsoleType(nameof(DSLite), 25);
        public static readonly ConsoleType DSI    = new ConsoleType(nameof(DSI), 26);
        public static readonly ConsoleType DS3D   = new ConsoleType(nameof(DS3D), 27);
        #endregion

        public ConsoleType(string name, ushort value)
            : base(name, value)
        {
        }
        
        public static ConsoleKind GetKind(ConsoleType consoleType)
        {
            if (consoleType == null)
                throw new ArgumentNullException(nameof(consoleType));
            
            var kind = ConsoleKind.None;
            
            // Home consoles.
            if (consoleType >= NES && consoleType <= XBSX)
                kind |= ConsoleKind.Home;
            
            // Handhelds. Take hybrids such as Switch into account.
            if (consoleType >= Vita && consoleType <= DS3D || (new [] { SwitchLite, Switch }).Contains(consoleType))
                kind |= ConsoleKind.Handheld;
            
            return kind;
        }
        
        public static Vendor GetVendor(ConsoleType consoleType)
        {
            if (consoleType == null)
                throw new ArgumentNullException(nameof(consoleType));
            
            // Nintendo.
            if ((consoleType >= NES && consoleType <= SwitchLite) || (consoleType >= GB && consoleType <= DS3D))
                return Vendor.Nintendo;
            
            // Sony.
            if ((consoleType >= PS1 && consoleType <= PS5) || (consoleType >= PSP && consoleType <= Vita))
                return Vendor.Nintendo;

            // Microsoft.
            if (consoleType >= XB && consoleType <= XBSX)
                return Vendor.Nintendo;
            
            throw new ArgumentException($"Could not determine vendor for console {consoleType.Name}", nameof(consoleType));
        }
    }
}