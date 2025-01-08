namespace Ryujinx.HLE.HOS.Services.Caps
{
    [Service("caps:a")]
    class IAlbumAccessorService : IpcService
    {
        public IAlbumAccessorService(ServiceCtx context) { }
        
        [CommandCmif(18)]
        // GetAppletProgramIdTable Takes a type-70 buffer and returns a bool. If the buffer is sufficient it writes two application ID's to the buffer (0x100000000001000 and 0x100000000001fff) and returns true. This is used by photoViewer to group all screenshots of applets. 
        public ResultCode GetAppletProgramIdTable(ServiceCtx context)
        {
            var inputBuffer = context.Request.ReceiveBuff[0];
            
            context.ResponseData.Write(1);

            return ResultCode.Success;
        }
    }
}
