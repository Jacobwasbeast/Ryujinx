using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Services.Audctl
{
    [Service("audctl")]
    class IAudioController : IpcService
    {
        private KEvent _notificationEvent;
        private int _notificationEventHandle;
        public IAudioController(ServiceCtx context)
        {
            _notificationEvent = new KEvent(context.Device.System.KernelContext);
            _notificationEventHandle = -1;
        }

        [CommandCmif(13)]
        // GetOutputModeSetting() -> u32
        public ResultCode GetOutputModeSetting(ServiceCtx context)
        {
            context.ResponseData.Write(1);
            return ResultCode.Success;
        }
        
        [CommandCmif(18)]
        // GetHeadphoneOutputLevelMode() -> u32
        public ResultCode GetHeadphoneOutputLevelMode(ServiceCtx context)
        {
            context.ResponseData.Write(0);
            return ResultCode.Success;
        }
        
        [CommandCmif(31)]
        // IsSpeakerAutoMuteEnabled() -> bool
        public ResultCode IsSpeakerAutoMuteEnabled(ServiceCtx context)
        {
            context.ResponseData.Write(true);

            Logger.Stub?.PrintStub(LogClass.ServiceAm);

            return ResultCode.Success;
        }
        
        [CommandCmif(34)]
        // AcquireTargetNotification() -> handle<copy>
        public ResultCode AcquireTargetNotification(ServiceCtx context)
        {
            if (_notificationEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_notificationEvent.ReadableEvent, out _notificationEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }

            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_notificationEventHandle);

            Logger.Stub?.PrintStub(LogClass.Audio);

            return ResultCode.Success;
        }
    }
}
