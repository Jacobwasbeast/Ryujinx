using Ryujinx.Common.Logging;

namespace Ryujinx.HLE.HOS.Services.Caps
{
    [Service("caps:a")]
    class IAlbumAccessorService : IpcService
    {
        public IAlbumAccessorService(ServiceCtx context) { }
        
        [CommandCmif(18)]
        // GetAppletProgramIdTable(buffer<nn::caps::ProgramIdTable>) -> bool
        public ResultCode GetAppletProgramIdTable(ServiceCtx context)
        {
            // TODO: Implement this properly.
            Logger.Stub?.PrintStub(LogClass.ServiceCaps);
            context.ResponseData.Write(false);

            return ResultCode.Success;
        }
    }
}
