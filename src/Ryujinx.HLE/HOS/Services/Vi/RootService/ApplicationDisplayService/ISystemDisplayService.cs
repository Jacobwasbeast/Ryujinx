using Ryujinx.Common.Logging;
using System;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Services.Vi.RootService.ApplicationDisplayService
{
    class ISystemDisplayService : IpcService
    {
#pragma warning disable IDE0052 // Remove unread private member
        private readonly IApplicationDisplayService _applicationDisplayService;
#pragma warning restore IDE0052

        public ISystemDisplayService(IApplicationDisplayService applicationDisplayService)
        {
            _applicationDisplayService = applicationDisplayService;
        }

        [CommandCmif(2205)]
        // SetLayerZ(u64, u64)
        public ResultCode SetLayerZ(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceVi);

            return ResultCode.Success;
        }

        [CommandCmif(2207)]
        // SetLayerVisibility(b8, u64)
        public ResultCode SetLayerVisibility(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceVi);

            return ResultCode.Success;
        }

        [CommandCmif(2312)] // 1.0.0-6.2.0
        // CreateStrayLayer(u32, u64) -> (u64, u64, buffer<bytes, 6>)
        public ResultCode CreateStrayLayer(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceVi);

            return _applicationDisplayService.CreateStrayLayer(context);
        }
        
        [CommandCmif(3000)]
        // ListDisplayModes(u64) -> (u64, buffer<nn::vi::DisplayModeInfo, 6>)
        public ResultCode ListDisplayModes(ServiceCtx context)
        {
            ulong displayId = context.RequestData.ReadUInt64();
            int outCount = 1;
            ulong bufferPosition = context.Request.ReceiveBuff[0].Position;
            ulong bufferLen = context.Request.ReceiveBuff[0].Size;
            DisplayMode[] displayModes = new DisplayMode[outCount];
            displayModes[0] = new DisplayMode
            {
                Width = 1280,
                Height = 720,
                RefreshRate = 60.0f,
                Unknown = 0
            };
            byte[] displayModeBytes = new byte[outCount * 0x10];
            MemoryMarshal.Cast<DisplayMode, byte>(displayModes).CopyTo(displayModeBytes);
            context.Memory.Write(bufferPosition, displayModeBytes);
            context.ResponseData.Write(outCount);
            Logger.Stub?.PrintStub(LogClass.ServiceVi);
            return ResultCode.Success;
        }
        
        struct DisplayMode
        {
            public uint Width { get; set; }
            public uint Height { get; set; }
            public float RefreshRate { get; set; }
            public uint Unknown { get; set; }
        }
        
        [CommandCmif(3200)]
        // GetDisplayMode(u64) -> nn::vi::DisplayModeInfo
        public ResultCode GetDisplayMode(ServiceCtx context)
        {
            ulong displayId = context.RequestData.ReadUInt64();

            (ulong width, ulong height) = AndroidSurfaceComposerClient.GetDisplayInfo(context, displayId);

            context.ResponseData.Write((uint)width);
            context.ResponseData.Write((uint)height);
            context.ResponseData.Write(60.0f);
            context.ResponseData.Write(0);

            Logger.Stub?.PrintStub(LogClass.ServiceVi);

            return ResultCode.Success;
        }
    }
}
