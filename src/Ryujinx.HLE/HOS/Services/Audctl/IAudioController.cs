using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Services.Audctl
{
    [Service("audctl")]
    class IAudioController : IpcService
    {
        public IAudioController(ServiceCtx context) { }

        [CommandCmif(34)]
        // AcquireTargetNotification() -> handle<copy>
        public ResultCode AcquireTargetNotification(ServiceCtx context)
        {
            Result code = context.Device.System.KernelContext.Syscall.CreateEvent(out int wEventHandle, out int rEventHandle);
            if (code == Result.Success)
            {
                context.Response.HandleDesc = IpcHandleDesc.MakeCopy(wEventHandle);
            }

            return ResultCode.Success;
        }
    }
}
