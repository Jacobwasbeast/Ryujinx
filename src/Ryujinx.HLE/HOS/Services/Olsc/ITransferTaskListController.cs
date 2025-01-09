namespace Ryujinx.HLE.HOS.Services.Olsc
{
    class ITransferTaskListController : IpcService
    {
        // TODO: Implement this service properly I believe they share the same instances of INativeHandleHolder.
        public ITransferTaskListController() { }
        
        [CommandCmif(5)]
        // GetTransferTaskEndEventNativeHandleHolder() -> INativeHandleHolder
        public ResultCode GetTransferTaskEndEventNativeHandleHolder(ServiceCtx context)
        {
            MakeObject(context, new INativeHandleHolder(context));

            return ResultCode.Success;
        }
        
        [CommandCmif(9)]
        // GetTransferTaskStartEventNativeHandleHolder() -> INativeHandleHolder
        public ResultCode GetTransferTaskStartEventNativeHandleHolder(ServiceCtx context)
        {
            MakeObject(context, new INativeHandleHolder(context));

            return ResultCode.Success;
        }
    }
}
