using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;
using System;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class IHomeMenuFunctions : IpcService
    {
        private ulong _pid;
        private int _channelEventHandle;

        public IHomeMenuFunctions(Horizon system, ulong pid)
        {
            _pid = pid;
        }

        [CommandCmif(10)]
        // RequestToGetForeground()
        public ResultCode RequestToGetForeground(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            context.Device.System.WindowSystem.RequestApplicationToGetForeground(_pid);
            context.Device.System.GetAppletState(_pid).SetFocusForce(true);
            
            return ResultCode.Success;
        }

        [CommandCmif(11)]
        // LockForeground()
        public ResultCode LockForeground(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            context.Device.System.WindowSystem.RequestLockHomeMenuIntoForeground();
            
            return ResultCode.Success;
        } 
        
        [CommandCmif(12)]
        // UnlockForeground()
        public ResultCode UnlockForeground(ServiceCtx context)
        {
            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            context.Device.System.WindowSystem.RequestUnlockHomeMenuFromForeground();

            return ResultCode.Success;
        }
        
        [CommandCmif(21)]
        // GetPopFromGeneralChannelEvent() -> handle<copy>
        public ResultCode GetPopFromGeneralChannelEvent(ServiceCtx context)
        {
            if (_channelEventHandle == 0)
            {
                if (context.Process.HandleTable.GenerateHandle(
                        context.Device.System.GeneralChannelEvent.ReadableEvent,
                        out _channelEventHandle) != Result.Success)
                {
                    throw new InvalidOperationException("Out of handles!");
                }
            }

            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_channelEventHandle);

            Logger.Stub?.PrintStub(LogClass.ServiceAm);

            return ResultCode.Success;
        }
        
        [CommandCmif(31)]
        // GetWriterLockAccessorEx(i32) -> object<nn::am::service::ILockAccessor>
        public ResultCode GetWriterLockAccessorEx(ServiceCtx context)
        {
            int lockId = context.RequestData.ReadInt32();

            MakeObject(context, new ILockAccessor(lockId, context.Device.System));

            Logger.Stub?.PrintStub(LogClass.ServiceAm);
            
            return ResultCode.Success;
        }
    }
}
