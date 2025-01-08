using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Services.BluetoothManager
{
    class IBtmSystemCore : IpcService
    {
        public IBtmSystemCore(ServiceCtx context) { }
        
        [CommandCmif(6)]
        // IsRadioEnabled() -> bool
        public ResultCode IsRadioEnabled(ServiceCtx context)
        {
            context.ResponseData.Write(true);

            return ResultCode.Success;
        }
        
        [CommandCmif(7)]
        // AcquireRadioEvent() -> u8, handle<copy>
        public ResultCode AcquireRadioEvent(ServiceCtx context)
        {
            Result code = context.Device.System.KernelContext.Syscall.CreateEvent(out int wEventHandle, out int rEventHandle);
            context.ResponseData.Write(true);
            if (code == Result.Success)
            {
                context.Response.HandleDesc = IpcHandleDesc.MakeCopy(wEventHandle);
            }
            
            return ResultCode.Success;
        }
    }
}
