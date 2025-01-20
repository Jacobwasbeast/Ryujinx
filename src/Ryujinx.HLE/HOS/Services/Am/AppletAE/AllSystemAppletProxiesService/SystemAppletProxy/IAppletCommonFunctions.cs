using Ryujinx.Common.Logging;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class IAppletCommonFunctions : IpcService
    {
        public IAppletCommonFunctions() { }
        
        [CommandCmif(70)]
        // SetCpuBoostRequestPriority(s32) -> void
        public ResultCode SetCpuBoostRequestPriority(ServiceCtx context)
        {
            // TODO: Implement this if needed.
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            return ResultCode.Success;
        }
    }
}
