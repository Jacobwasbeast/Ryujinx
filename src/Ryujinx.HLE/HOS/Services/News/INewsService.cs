namespace Ryujinx.HLE.HOS.Services.News
{
    class INewsService : IpcService
    {
        public INewsService(ServiceCtx context) { }
        
        [CommandCmif(30100)]
        // GetSubscriptionStatus() -> u32
        public ResultCode GetSubscriptionStatus(ServiceCtx context)
        {
            // TODO: Implement news service.
            return ResultCode.Success;
        }
        
    }
}
