using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Services.BluetoothManager
{
    class IBtmSystemCore : IpcService
    {
        private KEvent _radioEvent;
        private int _radioEventHandle;
        public IBtmSystemCore(ServiceCtx context)
        {
            _radioEvent = new KEvent(context.Device.System.KernelContext);
            _radioEventHandle = -1;
        }
        
        [CommandCmif(6)]
        // IsRadioEnabled() -> b8
        public ResultCode IsRadioEnabled(ServiceCtx context)
        {
            context.ResponseData.Write(true);

            return ResultCode.Success;
        }
        
        [CommandCmif(7)]
        // AcquireRadioEvent() -> u8, handle<copy>
        public ResultCode AcquireRadioEvent(ServiceCtx context)
        {
            if (_radioEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_radioEvent.ReadableEvent, out _radioEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }
            // AcquireRadioEvent also returns a u8
            context.ResponseData.Write(true);
            
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_radioEventHandle);

            Logger.Stub?.PrintStub(LogClass.Service);
            
            return ResultCode.Success;
        }
    }
}
