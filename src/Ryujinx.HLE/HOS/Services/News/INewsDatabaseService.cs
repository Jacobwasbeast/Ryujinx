namespace Ryujinx.HLE.HOS.Services.News
{
    class INewsDatabaseService : IpcService
    {
        public INewsDatabaseService(ServiceCtx context) { }
        
        [CommandCmif(1)]
        // Count(buffer<nn::news::detail::Count>) -> u32
        public ResultCode Count(ServiceCtx context)
        {
            // TODO: Implement news database count
            context.ResponseData.Write((uint)0);
            return ResultCode.Success;
        }
        
    }
}
