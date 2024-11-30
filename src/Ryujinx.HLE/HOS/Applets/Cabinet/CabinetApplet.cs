using LibHac.Ncm;
using LibHac.Tools.FsSystem.NcaUtils;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using Ryujinx.HLE.FileSystem;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Ryujinx.HLE.HOS.Services.Nfc.Nfp.NfpManager;
using System.Runtime.CompilerServices;
using static LibHac.FsSrv.Impl.AccessControlBits;
using Ryujinx.Graphics.Gpu;
using Ryujinx.Common.Configuration;
using Ryujinx.HLE.HOS.Services.Hid;
namespace Ryujinx.HLE.HOS.Applets.Cabinet
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
            byte[] launchParams = _normalSession.Pop();
            byte[] startParam = _normalSession.Pop();
            string contentPath = _system.ContentManager.GetInstalledContentPath(0x0100000000001002, StorageId.BuiltInSystem, NcaContentType.Program);
            NfpDevice nfpDevice = new NfpDevice();
            nfpDevice.AmiiboId = AppDataManager.LastScannedAmiiboId;
            nfpDevice.UseRandomUuid = false;
            nfpDevice.State = NfpDeviceState.Initialized;
            nfpDevice.Handle = PlayerIndex.Player1;
            _system.NfpDevices.Add(nfpDevice);


            Console.WriteLine("CabinetApplet Start");
            Console.WriteLine($"LaunchParams: {BitConverter.ToString(launchParams)}");
            Console.WriteLine($"StartParam: {BitConverter.ToString(startParam)}");
            StartParamForAmiiboSettings startParamForAmiiboSettings = StructConverter.BytesToStruct<StartParamForAmiiboSettings>(startParam);
            switch (startParamForAmiiboSettings.Type)
            {
                case 0:
                    StartNicknameAndOwnerSettings(startParamForAmiiboSettings);
                    break;
                case 1:
                    StartFormatter(startParamForAmiiboSettings);
                    break;
                case 3:
                    StartFormatter(startParamForAmiiboSettings);
                    break;
                default:
                    Logger.Error?.Print(LogClass.ServiceAm, $"Unknown AmiiboSettings type: {startParamForAmiiboSettings.Type}");
                    break;
            }
            _normalSession.Push(BuildResponse(startParamForAmiiboSettings));
            AppletStateChanged?.Invoke(this, null);

            _system.ReturnFocus();
            return ResultCode.Success;
        }
        public void StartFormatter(StartParamForAmiiboSettings startParamForAmiibo)
        {
            startParamForAmiibo.RegisterInfo = new RegisterInfo();
        }
        public void StartNicknameAndOwnerSettings(StartParamForAmiiboSettings startParamForAmiibo)
        {
            RegisterInfo registerInfo = startParamForAmiibo.RegisterInfo;
            string newName = "Big Chungus";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(newName);
            bytes.CopyTo(registerInfo.Nickname.AsSpan());
            startParamForAmiibo.RegisterInfo = registerInfo;
        }
        private static T ReadStruct<T>(byte[] data) where T : struct
        {
            return MemoryMarshal.Read<T>(data.AsSpan());
        }
        private byte[] BuildResponse(StartParamForAmiiboSettings startParamForAmiibo)
        {
            ReturnValueForAmiiboSettings returnValue = new ReturnValueForAmiiboSettings
            {
                ReturnFlag = (byte)AmiiboSettingsReturnFlag.HasRegisterInfo,
                DeviceHandle = (int)PlayerIndex.Player1,
                RegisterInfo = startParamForAmiibo.RegisterInfo,
                TagInfo = new TagInfo(),
                IgnoredBySdk = new byte[0x24],
                Padding = new byte[3]
            };
            using MemoryStream stream = MemoryStreamManager.Shared.GetStream();
            using BinaryWriter writer = new(stream);
            writer.Write(StructConverter.StructToBytes(returnValue));
            return stream.ToArray();
        }
        public ResultCode GetResult()
        {
            return ResultCode.Success;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TagInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x58)] // Size 0x58
            public byte[] Data;
        }

        // Define struct for nn::nfp::AmiiboSettingsStartParam
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AmiiboSettingsStartParam
        {
            // Offset 0x0, Size 0x8
            public ulong Unknown1;

            // Offset 0x8, Size 0x20
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public byte[] Unknown2;

            // Offset 0x28, Size 0x1
            public byte Unknown3;
        }

        // Define struct for nn::nfp::StartParamForAmiiboSettings
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct StartParamForAmiiboSettings
        {
            // Offset 0x0, Size 0x1
            public byte Unknown1;

            // Offset 0x1, Size 0x1
            public byte Type;

            // Offset 0x2, Size 0x1
            public byte Flags;

            // Offset 0x3, Size 0x1
            public byte Unknown2;  // This corresponds to #AmiiboSettingsStartParam + 0x28 (AmiiboSettingsStartParam.Unknown3)

            // Offset 0x4, Size 0x8
            public ulong Unknown3;  // This corresponds to #AmiiboSettingsStartParam + 0x0 (AmiiboSettingsStartParam.Unknown1)

            // Offset 0xC, Size 0x58
            public TagInfo TagInfo;  // Only enabled when flags bit 1 is set.

            // Offset 0x64, Size 0x100
            public RegisterInfo RegisterInfo;  // Only enabled when flags bit 2 is set.

            // Offset 0x164, Size 0x20
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public byte[] Unknown4;  // This corresponds to #StartParamForAmiiboSettings + 0x8

            // Offset 0x184, Size 0x24
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
            public byte[] Unknown5;  // Left all-zero by sdknso
        }

        // Define struct for nn::nfp::ReturnValueForAmiiboSettings
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ReturnValueForAmiiboSettings
        {
            // Offset 0x0, Size 0x1
            public byte ReturnFlag;  // #AmiiboSettingsReturnFlag

            // Offset 0x1, Size 0x3
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
            public byte[] Padding;  // Padding of 0x3 bytes

            // Offset 0x4, Size 0x8
            public ulong DeviceHandle;

            // Offset 0xC, Size 0x58
            public TagInfo TagInfo;

            // Offset 0x64, Size 0x100
            public RegisterInfo RegisterInfo;  // Only available when flags bit 2 is set.

            // Offset 0x164, Size 0x24
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
            public byte[] IgnoredBySdk;  // Ignored by sdknso
        }

        // Enum for AmiiboSettingsReturnFlag
        public enum AmiiboSettingsReturnFlag
        {
            Cancel = 0,
            HasTagInfo = 2,
            HasRegisterInfo = 4,
            HasTagInfoAndRegisterInfo = 6
        }

        public static class StructConverter
        {
            // Convert Struct to Byte Array
            public static byte[] StructToBytes<T>(T obj)
            {
                int size = Marshal.SizeOf(obj);
                byte[] bytes = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);

                try
                {
                    Marshal.StructureToPtr(obj, ptr, false);
                    Marshal.Copy(ptr, bytes, 0, size);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return bytes;
            }

            // Convert Byte Array to Struct
            public static T BytesToStruct<T>(byte[] bytes)
            {
                int size = Marshal.SizeOf(typeof(T));
                if (bytes.Length != size)
                    throw new ArgumentException("Byte array size does not match struct size.");

                IntPtr ptr = Marshal.AllocHGlobal(size);

                try
                {
                    Marshal.Copy(bytes, 0, ptr, size);
                    return (T)Marshal.PtrToStructure(ptr, typeof(T));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }
}
