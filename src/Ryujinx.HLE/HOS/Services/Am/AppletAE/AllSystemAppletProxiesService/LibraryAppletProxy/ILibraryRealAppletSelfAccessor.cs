using LibHac;
using Ryujinx.Common;
using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Applets.Real;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy;
using Ryujinx.HLE.HOS.SystemState;
using System;
using Result = Ryujinx.Horizon.Common.Result;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.LibraryAppletProxy
{
    class ILibraryRealAppletSelfAccessor : IpcService
    {
        public Horizon System { get; set; }
        public ulong ProgramId { get; set; }
        public RealApplet _appletStandalone => System.RealAppletManager.GetApplet(ProgramId);

        public KEvent _popInteractiveInDataEvent;
        public int _popInteractiveInDataEventHandle;

        public ILibraryRealAppletSelfAccessor(ServiceCtx context)
        {
            System = context.Device.System;
            ProgramId = context.Device.Processes.ActiveApplication.ProgramId;

            _popInteractiveInDataEvent = new KEvent(System.KernelContext);
            _popInteractiveInDataEventHandle = -1;
        }

        [CommandCmif(0)]
        // PopInData() -> object<nn::am::service::IStorage>
        public ResultCode PopInData(ServiceCtx context)
        {
            if (_appletStandalone.InputBuffer.TryDequeue(out byte[] data))
            {
                MakeObject(context, new IStorage(data));
            }
            else
            {
                // TODO: Return error code when buffer is empty.
                throw new NotImplementedException();
            }

            return ResultCode.Success;
        }

        [CommandCmif(2)]
        // PopInteractiveInData() -> object<nn::am::service::IStorage>
        public ResultCode PopInteractiveInData(ServiceCtx context)
        {
            if (_appletStandalone.InteractiveBuffer.TryDequeue(out byte[] data))
            {
                MakeObject(context, new IStorage(data));
            }
            else
            {
                // TODO: Return error code when buffer is empty.
                throw new NotImplementedException();
            }

            return ResultCode.Success;
        }

        [CommandCmif(3)]
        // PushInteractiveOutData(IStorage) -> void
        public ResultCode PushInteractiveOutData(ServiceCtx context)
        {
            IStorage data = GetObject<IStorage>(context, 0);
            _appletStandalone.InteractiveBuffer.Enqueue(data.Data);
            return ResultCode.Success;
        }

        [CommandCmif(6)]
        // GetPopInteractiveInDataEvent() -> handle
        public ResultCode GetPopInteractiveInDataEvent(ServiceCtx context)
        {
            if (_popInteractiveInDataEventHandle == -1)
            {
                var result = context.Process.HandleTable.GenerateHandle(_popInteractiveInDataEvent.ReadableEvent,
                    out _popInteractiveInDataEventHandle);
                if (result != Result.Success)
                {
                    return (ResultCode)result.ErrorCode;
                }
            }

            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_popInteractiveInDataEventHandle);

            return ResultCode.Success;
        }

        [CommandCmif(10)]
        // End() -> void
        public ResultCode End(ServiceCtx context)
        {
            if (_appletStandalone.InputBuffer.IsEmpty)
            {
                _appletStandalone.InputBuffer.Enqueue(new byte[0]);
                Logger.Info?.Print(LogClass.Service, $"Input buffer empty");
            }

            _appletStandalone._stateChangedEvent.ReadableEvent.Signal();
            for (int i = 0; i < _appletStandalone.InputBuffer.Count; ++i)
            {
                if (_appletStandalone.InputBuffer.TryDequeue(out byte[] data))
                {
                    MakeObject(context, new IStorage(data));
                }
            }

            context.Device.System.SetFromAppletStateMgr(_appletStandalone.AppletManagerBefore);
            context.Device.System.TerminateCurrentProcess(context.Process.TitleId);
            context.Device.System.SurfaceFlinger.SetRenderLayer(_appletStandalone.LastRenderLayer);
            context.Device.Processes.SetLatestPID(_appletStandalone.BeforeId);
            context.Device.System.RealAppletManager.RemoveApplet(ProgramId);
            return ResultCode.Success;
        }

        [CommandCmif(11)]
        // GetLibraryAppletInfo() -> nn::am::service::LibraryAppletInfo
        public ResultCode GetLibraryAppletInfo(ServiceCtx context)
        {
            LibraryAppletInfo libraryAppletInfo = new()
            {
                AppletId = _appletStandalone.AppletId, LibraryAppletMode = _appletStandalone.LibraryAppletMode,
            };

            context.ResponseData.WriteStruct(libraryAppletInfo);

            return ResultCode.Success;
        }

        [CommandCmif(14)]
        // GetCallerAppletIdentityInfo() -> nn::am::service::AppletIdentityInfo
        public ResultCode GetCallerAppletIdentityInfo(ServiceCtx context)
        {
            AppletIdentifyInfo appletIdentifyInfo = new()
            {
                AppletId = AppletId.QLaunch, TitleId = 0x0100000000001000,
            };

            context.ResponseData.WriteStruct(appletIdentifyInfo);

            return ResultCode.Success;
        }

        [CommandCmif(19)]
        // GetDesirableKeyboardLayout() -> u32
        public ResultCode GetDesirableKeyboardLayout(ServiceCtx context)
        {
            context.ResponseData.Write((uint)KeyboardLayout.Default);
            return ResultCode.Success;
        }

        [CommandCmif(150)]
        // ShouldSetGpuTimeSliceManually() -> bool
        public ResultCode ShouldSetGpuTimeSliceManually(ServiceCtx context)
        {
            context.ResponseData.Write(false); // TODO: Implement this properly
            return ResultCode.Success;
        }
    }
}
