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
            context.ResponseData.Write(false);

            return ResultCode.Success;
        }
    }
}
