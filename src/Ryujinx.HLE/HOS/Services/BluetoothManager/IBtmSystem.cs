namespace Ryujinx.HLE.HOS.Services.BluetoothManager
{
    [Service("btm:sys")]
    class IBtmSystem : IpcService
    {
        public IBtmSystem(ServiceCtx context) { }
        
        [CommandCmif(0)]
        // GetCore() -> IBtmSystemCore
        public ResultCode GetCore(ServiceCtx context)
        {
            MakeObject(context, new IBtmSystemCore(context));

            return ResultCode.Success;
        }
        
        [CommandCmif(6)]
        // IsRadioEnabled() -> bool
        public ResultCode IsRadioEnabled(ServiceCtx context)
        {
            context.ResponseData.Write(false);

            return ResultCode.Success;
        }
    }
}
