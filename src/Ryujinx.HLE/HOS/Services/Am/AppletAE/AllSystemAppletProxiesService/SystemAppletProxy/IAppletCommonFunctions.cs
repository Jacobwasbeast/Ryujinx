using Ryujinx.Common.Logging;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class IAppletCommonFunctions : IpcService
    {
        public IAppletCommonFunctions() { }
        
        [CommandCmif(51)]
        // GetHomeButtonDoubleClickEnabled() -> bool
        public ResultCode GetHomeButtonDoubleClickEnabled(ServiceCtx context)
        {
            context.ResponseData.Write(true);
            Logger.Info?.PrintStub(LogClass.ServiceAm);
            return ResultCode.Success;
        }
        
        [CommandCmif(70)]
        // SetCpuBoostRequestPriority(s32) -> void
        public ResultCode SetCpuBoostRequestPriority(ServiceCtx context)
        {
            Logger.Info?.PrintStub(LogClass.ServiceAm);
            return ResultCode.Success;
        }
    }
}
