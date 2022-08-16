using System;
using System.Drawing;

namespace Inviscan.Models
{
    // TODO: install expressive enum extensions package and convert theses to that.
    public enum ConsoleType : ushort
    {
        #region Nintend home consoles
        NES = 0,
        SNES,
        N64,
        
        /// <summary>
        /// GameCube.
        /// </summary>
        GC,
        Wii,
        WiiU,
        Switch,
        SwitchLite,
        #endregion

        #region Nintendo handheld consoles
        /// <summary>
        /// Original GameBoy.
        /// </summary>
        GB,
        
        /// <summary>
        /// GameBoy Color.
        /// </summary>
        GBC,
        
        /// <summary>
        /// GameBoy Advance.
        /// </summary>
        GBA,
        GBASP,
        DS,
        DSLite,
        DSI,
        
        /// <summary>
        /// Nintendo 3DS.
        /// </summary>
        DS3D,
        #endregion

        #region Sony home consoles
        // Sony home consoles.
        PS1,
        PS2,
        PS3,
        PS4,
        PS5,
        #endregion

        #region Sony handheld consoles
        Vita,
        PSP,
        #endregion
        
        #region Microsoft home consoles
        XB,
        XB360,
        
        /// <summary>
        /// Xbox one.
        /// </summary>
        XBO,
        
        /// <summary>
        /// Xbox series X.
        /// </summary>
        XBSX
        #endregion
    }
    
    [Flags]
    public enum ConsoleKind : byte
    {
        None     = 0,
        Home     = (1 << 0),
        Handheld = (1 << 1)
    }
    
    public readonly struct Console
    {
        #region Properties
        public ConsoleType Type
        {
            get;
        }

        public ConsoleKind Kind
        {
            get;
        }

        public Vendor Vendor
        {
            get;
        }
        #endregion

        public Console(ConsoleType type, ConsoleKind kind, Vendor vendor)
        {
            Type   = type;
            Kind   = kind;
            Vendor = vendor;
        }
    }
}