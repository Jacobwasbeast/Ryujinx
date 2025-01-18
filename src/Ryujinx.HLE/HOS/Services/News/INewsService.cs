using Ryujinx.Common.Logging;

namespace Ryujinx.HLE.HOS.Services.News
{
    class INewsService : IpcService
    {
        public INewsService(ServiceCtx context) { }
        
        [CommandCmif(30100)]
        // GetSubscriptionStatus() -> u32
        public ResultCode GetSubscriptionStatus(ServiceCtx context)
        {
            // TODO: Implement this properly
            Logger.Stub?.PrintStub(LogClass.Service);
            context.ResponseData.Write((uint)0);
            return ResultCode.Success;
        }
        
    }
}
