namespace Ryujinx.HLE.HOS.Services.Ns
{
    class IDownloadTaskInterface : IpcService
    {
        public IDownloadTaskInterface(ServiceCtx context) { }
        
        [CommandCmif(707)]
        // EnableAutoCommit()
        public ResultCode EnableAutoCommit(ServiceCtx context)
        {
            return ResultCode.Success;
        }
    }
}
