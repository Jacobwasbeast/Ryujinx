using System.Collections.Generic;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE
{
    public class AppletIndexMap
    {
        public static Dictionary<int, ulong> _mapper = new Dictionary<int, ulong>()
        {
            { 0x02, 0x010000000000100C },
            { 0x03, 0x0100000000001000 },
            { 0x04, 0x0100000000001012 },
            { 0x0A, 0x0100000000001001 },
            { 0x0B, 0x0100000000001002 },
            { 0x0C, 0x0100000000001003 },
            { 0x0D, 0x0100000000001004 },
            { 0x0E, 0x0100000000001005 },
            { 0x0F, 0x0100000000001006 },
            { 0x10, 0x0100000000001007 },
            { 0x11, 0x0100000000001008 },
            { 0x12, 0x0100000000001009 },
            { 0x13, 0x010000000000100A },
            { 0x14, 0x010000000000100B },
            { 0x15, 0x010000000000100D },
            { 0x16, 0x010000000000100E },
            { 0x17, 0x010000000000100F },
            { 0x18, 0x0100000000001010 },
            { 0x19, 0x0100000000001011 },
            { 0x1A, 0x0100000000001013 },
            { 0x1B, 0x010000000000101A },
            { 0x1C, 0x010000000000101C },
            { 0x1D, 0x010000000000101D },
            { 0x1E, 0x0100000000001020 },
            { 0x1F, 0x010070000E3C0000 },
            { 0x20, 0x010086000E49C000 },
            { 0x21, 0x0100000000001038 },
            { 0x22, 0x0100000000001007 },
            { 0x32, 0x010000000000100F },
            { 0x33, 0x010000000000100F },
            { 0x35, 0x0100000000001010 },
            { 0x36, 0x0100000000001010 },
            { 0x37, 0x0100000000001010 },
            { 0x38, 0x0100000000001043 },
            { 0x50, 0x0100000000001007 },
            { 0x51, 0x0100000000001007 },
            { 0x3F1, 0x010000000000D619 },
            { 0x3F2, 0x010000000000D610 },
            { 0x3F3, 0x010000000000D611 },
            { 0x3F4, 0x010000000000D612 },
            { 0x3F5, 0x010000000000D613 },
            { 0x3F6, 0x010000000000D614 },
            { 0x3F7, 0x010000000000D615 },
            { 0x3F8, 0x010000000000D616 },
            { 0x3F9, 0x010000000000D617 },
            { 0x3FA, 0x010000000000D60A },
            { 0x3FB, 0x010000000000D60B },
            { 0x3FC, 0x010000000000D60C },
            { 0x3FD, 0x010000000000D60D },
            { 0x3FE, 0x010000000000D60E },
            { 0x700000C8, 0x010000000000D65B },
            { 0x700000C9, 0x010000000000D65C },
            { 0x700000DC, 0x010000000000D619 },
            { 0x700000E6, 0x010000000000D610 },
            { 0x700000E7, 0x010000000000D611 },
            { 0x700000E8, 0x010000000000D612 },
            { 0x700000E9, 0x010000000000D613 },
            { 0x700000EA, 0x010000000000D614 },
            { 0x700000EB, 0x010000000000D615 },
            { 0x700000EC, 0x010000000000D616 },
            { 0x700000ED, 0x010000000000D617 },
            { 0x700000F0, 0x010000000000D60A },
            { 0x700000F1, 0x010000000000D60B },
            { 0x700000F2, 0x010000000000D60C },
            { 0x700000F3, 0x010000000000D60D },
            { 0x700000F4, 0x010000000000D60E }
        };

        // TODO: Find the rest of the applets that require common arguments.
        public static List<ulong> _shouldProvideCommonArguments = new List<ulong>{
            0x010000000000100D
        };
        
        public static List<ulong> _partialForeground = new List<ulong>{
            0x0100000000001008
        };

        public static ulong GetAppletProgramId(int index)
        {
            if (_mapper.TryGetValue(index, out ulong programId))
                return programId;
            return 0;
        }

        internal static AppletId GetAppletId(ulong programId)
        {
            foreach (KeyValuePair<int, ulong> entry in _mapper)
            {
                if (entry.Value == programId)
                    return (AppletId)entry.Key;
            }
            return AppletId.Application;
        }
        
        public static bool ShouldProvideCommonArguments(ulong programId)
        {
            return _shouldProvideCommonArguments.Contains(programId);
        }
        
        public static bool IsPartialForeground(ulong programId)
        {
            return _partialForeground.Contains(programId);
        }
    }
}
