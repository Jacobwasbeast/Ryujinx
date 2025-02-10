namespace Ryujinx.HLE.HOS.Services.Ns
{
    class IDynamicRightsInterface : IpcService
    {
        [CommandCmif(5)]
        // VerifyActivatedRightsOwners(u64)
        public ResultCode VerifyActivatedRightsOwners(ServiceCtx context) => ResultCode.Success;


        [CommandCmif(13)]
        // GetRunningApplicationStatus() -> nn::ns::RunningApplicationStatus
        public ResultCode GetRunningApplicationStatus(ServiceCtx context)
        {
            context.ResponseData.Write(0);
            return ResultCode.Success;
        }
        
    }
}
