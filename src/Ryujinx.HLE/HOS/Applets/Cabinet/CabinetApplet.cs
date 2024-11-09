using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Applets
{
    internal class CabinetApplet : IApplet
    {
        private readonly Horizon _system;
        private AppletSession _normalSession;

        public event EventHandler AppletStateChanged;

        public CabinetApplet(Horizon system)
        {
            _system = system;
        }

        public ResultCode Start(AppletSession normalSession, AppletSession interactiveSession)
        {
            _normalSession = normalSession;

            // Retrieve and parse the parameters
            byte[] launchParams = _normalSession.Pop();
            StartParamForAmiiboSettings startParams = new StartParamForAmiiboSettings();

            startParams.Type = StartParamForAmiiboSettings.AmiiboType.StartNicknameAndOwnerSettings;
            startParams.Flags = 0x2; 

            Logger.Stub?.PrintStub(LogClass.ServiceNfc, $"Cabinet Applet Type: {startParams.Type}, Flags: {startParams.Flags}");

            // Populate and send a dummy response for the applet, for now
            _normalSession.Push(BuildResponse());
            AppletStateChanged?.Invoke(this, null);

            _system.ReturnFocus();

            return ResultCode.Success;
        }

        private static T ReadStruct<T>(byte[] data) where T : struct
        {
            return MemoryMarshal.Read<T>(data.AsSpan());
        }

        private void HandleNicknameAndOwnerSettings()
        {
            Logger.Info?.Print(LogClass.ServiceNfp, "Handling Nickname and Owner Settings...");
            // TODO: Implement handling logic here
        }

        private void HandleGameDataEraser()
        {
            Logger.Info?.Print(LogClass.ServiceNfp, "Handling Game Data Eraser...");
            // TODO: Implement handling logic here
        }

        private void HandleRestorer()
        {
            Logger.Info?.Print(LogClass.ServiceNfp, "Handling Restorer...");
            // TODO: Implement handling logic here
        }

        private void HandleFormatter()
        {
            Logger.Info?.Print(LogClass.ServiceNfp, "Handling Formatter...");
            // TODO: Implement handling logic here
        }

        private static byte[] BuildResponse()
        {
            using MemoryStream stream = MemoryStreamManager.Shared.GetStream();
            using BinaryWriter writer = new(stream);

            writer.Write((ulong)ResultCode.Success);

            return stream.ToArray();
        }

        public ResultCode GetResult()
        {
            return ResultCode.Success;
        }
    }

    // Struct definitions based on the switchbrew.org/wiki/Controller_Applet

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AmiiboSettingsStartParam
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x8)]
        public byte[] Reserved0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] Reserved1;

        public byte Flags;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StartParamForAmiiboSettings
    {
        public byte LeftAtZero;
        public AmiiboType Type; // This should map to an enum representing different types
        public byte Flags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x8)]
        public byte[] AmiiboSettingsStartParamOffset0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x58)]
        public byte[] TagInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] RegisterInfo;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] StartParamOffset8;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
        public byte[] AllZero;

        public enum AmiiboType : byte
        {
            StartNicknameAndOwnerSettings = 0,
            StartGameDataEraser = 1,
            StartRestorer = 2,
            StartFormatter = 3
        }
    }
}
