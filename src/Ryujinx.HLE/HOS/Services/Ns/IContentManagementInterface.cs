using Ryujinx.Common.Logging;

namespace Ryujinx.HLE.HOS.Services.Ns
{
    class IContentManagementInterface : IpcService
    {
        public IContentManagementInterface(ServiceCtx context) { }
        
        [CommandCmif(43)]
        // CheckSdCardMountStatus()
        public ResultCode CheckSdCardMountStatus(ServiceCtx context)
        {
            return ResultCode.Success;
        }
        
        // TODO: Implement proper storage space implementation.
        const long StorageSpace = 30000000000L;
        [CommandCmif(47)]
        // GetTotalSpaceSize(u8 storage_id) -> s64
        public ResultCode GetTotalSpaceSize(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.Service);
            long storageId = context.RequestData.ReadByte();
            context.ResponseData.Write(StorageSpace);
            return ResultCode.Success;
        }
        
        [CommandCmif(48)]
        // GetFreeSpaceSize(u8 storage_id) -> s64
        public ResultCode GetFreeSpaceSize(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.Service);
            long storageId = context.RequestData.ReadByte();
            long freeSpace = (long)(StorageSpace * 0.5);
            context.ResponseData.Write(freeSpace);
            return ResultCode.Success;
        }
    }
}
