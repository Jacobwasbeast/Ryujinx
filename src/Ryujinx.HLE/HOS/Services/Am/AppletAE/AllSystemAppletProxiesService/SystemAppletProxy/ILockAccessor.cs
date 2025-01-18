using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class ILockAccessor : IpcService
    {
        KEvent _lockChangedEvent;
        int _lockChangedEventHandle;
        bool _lockState;
        public ILockAccessor(ServiceCtx context)
        {
            _lockChangedEvent = new KEvent(context.Device.System.KernelContext);
            _lockChangedEventHandle = -1;
            _lockState = false;
        }
        
        [CommandCmif(1)]
        // TryLock(u8) -> bool, handle<copy>
        public ResultCode TryLock(ServiceCtx context)
        {
            byte flags = context.RequestData.ReadByte();
            bool lockState = false;
            if ((flags & 1) != 0)
            {
                lockState = _lockState;
                _lockState = true;
            }
            context.ResponseData.Write(lockState);
            if (_lockChangedEventHandle == -1)
            {
                if (context.Process.HandleTable.GenerateHandle(_lockChangedEvent.ReadableEvent, out _lockChangedEventHandle) != Result.Success)
                {
                    return ResultCode.NotAvailable;
                }
            }
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_lockChangedEventHandle);
            
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            return ResultCode.Success;
        }
        
        [CommandCmif(2)]
        // Unlock()
        public ResultCode Unlock(ServiceCtx context)
        {
            _lockState = false;
            _lockChangedEvent.ReadableEvent.Signal();
            return ResultCode.Success;
        }
        
        [CommandCmif(3)]
        // GetEvent() -> handle<copy>
        public ResultCode GetEvent(ServiceCtx context)
        {
            if (_lockChangedEventHandle == -1)
            {
                if (context.Process.HandleTable.GenerateHandle(_lockChangedEvent.ReadableEvent, out _lockChangedEventHandle) != Result.Success)
                {
                    return ResultCode.NotAvailable;
                }
            }
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_lockChangedEventHandle);
            
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            return ResultCode.Success;
        }
        
        [CommandCmif(4)]
        // IsLocked() -> bool
        public ResultCode IsLocked(ServiceCtx context)
        {
            context.ResponseData.Write(_lockState);
            return ResultCode.Success;
        }
    }
}
