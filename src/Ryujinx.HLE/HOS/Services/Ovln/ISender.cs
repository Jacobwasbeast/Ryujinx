using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;
namespace Ryujinx.HLE.HOS.Services.Ovln
{
    class ISender : IpcService
    {
        [CommandCmif(0)]
        // Send
        public ResultCode Send(ServiceCtx context)
        {
            return ResultCode.Success;
        }
        [CommandCmif(2)]
        // GetUnreceivedMessageCount() -> u32
        public ResultCode GetUnreceivedMessageCount(ServiceCtx context)
        {
            context.ResponseData.Write(0);
            return ResultCode.Success;
        }
    }
}
