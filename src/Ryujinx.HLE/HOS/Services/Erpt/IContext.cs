namespace Ryujinx.HLE.HOS.Services.Erpt
{
    [Service("erpt:c")]
    class IContext : IpcService
    {
        public IContext(ServiceCtx context) { }
        
        [CommandCmif(0)]
        // SubmitContext Takes two type-0x5 input buffers #ContextEntry and FieldList. No output. 
        public ResultCode SubmitContext(ServiceCtx context)
        {
            // TODO: Figure out what this command does
            return ResultCode.Success;
        }
    }
}
