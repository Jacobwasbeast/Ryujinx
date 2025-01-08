using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;
namespace Ryujinx.HLE.HOS.Services.Ovln
{
    class IReceiver : IpcService
    {
        [CommandCmif(0)]
        // AddSource()
        public ResultCode AddSource(ServiceCtx context)
        {
            return ResultCode.Success;
        }
        [CommandCmif(2)]
        // GetReceiveEventHandle() -> handle<copy>
        public ResultCode GetReceiveEventHandle(ServiceCtx context)
        {
            int receiveEventHandle = 0;
            var receiveEvent = new KEvent(context.Device.System.KernelContext);
            
            if (context.Process.HandleTable.GenerateHandle(receiveEvent.ReadableEvent, out receiveEventHandle) != Result.Success)
            {
                return ResultCode.NotAllocated;
            }
            
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(receiveEventHandle);
            return ResultCode.Success;
        }
    }
}
