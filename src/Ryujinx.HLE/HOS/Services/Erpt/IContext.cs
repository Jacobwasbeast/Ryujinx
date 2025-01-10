using Ryujinx.Common.Logging;

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
            Logger.Info?.PrintStub(LogClass.Service, $"ContextEntry size: {context.Request.SendBuff[0].Size}");
            Logger.Info?.PrintStub(LogClass.Service, $"FieldList size: {context.Request.SendBuff[1].Size}");
            return ResultCode.Success;
        }
    }
}
