using Ryujinx.Common;
using Ryujinx.Common.Configuration;
using Ryujinx.HLE.HOS.Ipc;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.LibraryAppletProxy
{
    class ILibraryAppletSelfAccessor : IpcService
    {
        private readonly AppletStandalone _appletStandalone = new();

        public ILibraryAppletSelfAccessor(ServiceCtx context)
        {
            if (context.Device.Processes.ActiveApplication.ProgramId == 0x0100000000001009)
            {
                _appletStandalone = new AppletStandalone()
                {
                    AppletId = AppletId.MiiEdit,
                    LibraryAppletMode = LibraryAppletMode.AllForeground,
                };

                byte[] miiEditInputData = new byte[0x100];
                miiEditInputData[0] = 0x03; // Hardcoded unknown value.

                _appletStandalone.InputData.Enqueue(miiEditInputData);
            }
            else if (context.Device.Processes.ActiveApplication.ProgramId == 0x0100000000001002)
            {
                _appletStandalone = new AppletStandalone()
                {
                    AppletId = AppletId.Cabinet,
                    LibraryAppletMode = LibraryAppletMode.AllForeground,
                };
                byte[] firstInput;
                AppDataManager.inputData.TryDequeue(out firstInput);
                _appletStandalone.InputData.Enqueue(firstInput);
                byte[] secondInput;
                AppDataManager.inputData.TryDequeue(out secondInput);
                _appletStandalone.InputData.Enqueue(secondInput);
            }
            //    AppletId applet = Enum.Parse<AppletId>(AppDataManager.applet);
            //_appletStandalone = new AppletStandalone()
            //{
            //    AppletId = applet,
            //    LibraryAppletMode = LibraryAppletMode.AllForeground,
            //};
            //byte[] miiEditInputData = new byte[0x100];
            //miiEditInputData[0] = 0x03; // Hardcoded unknown value.
            //_appletStandalone.InputData.Enqueue(miiEditInputData);
            //while (!AppDataManager.inputData.IsEmpty)
            //{
            //    byte[] data;
            //    AppDataManager.inputData.TryDequeue(out data);
            //    _appletStandalone.InputData.Enqueue(data);
            //}
         }

        [CommandCmif(0)]
        // PopInData() -> object<nn::am::service::IStorage>
        public ResultCode PopInData(ServiceCtx context)
        {
            if (_appletStandalone.InputData.TryDequeue(out byte[] appletData))
            {
                MakeObject(context, new IStorage(appletData));
                return ResultCode.Success;
            }

            return ResultCode.NotAvailable;
        }

        [CommandCmif(1)]
        // PushOutData(object<nn::am::service::IStorage>)
        public ResultCode PushOutData(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(2)]
        // PopInteractiveInData() -> object<nn::am::service::IStorage>
        public ResultCode PopInteractiveInData(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(3)]
        // PushInteractiveOutData(object<nn::am::service::IStorage>)
        public ResultCode PushInteractiveOutData(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(5)]
        // GetPopInDataEvent() -> handle<copy>
        public ResultCode GetPopInDataEvent(ServiceCtx context)
        {
            // Stub implementation for event handle
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(0);
            return ResultCode.Success;
        }
        [CommandCmif(6)]
        // GetPopInteractiveInDataEvent() -> handle<copy>
        public ResultCode GetPopInteractiveInDataEvent(ServiceCtx context)
        {
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(0);
            return ResultCode.Success;
        }
        [CommandCmif(10)]
        // ExitProcessAndReturn()
        public ResultCode ExitProcessAndReturn(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(11)]
        // GetLibraryAppletInfo() -> nn::am::service::LibraryAppletInfo
        public ResultCode GetLibraryAppletInfo(ServiceCtx context)
        {
            LibraryAppletInfo libraryAppletInfo = new()
            {
                AppletId = _appletStandalone.AppletId,
                LibraryAppletMode = _appletStandalone.LibraryAppletMode,
            };

            context.ResponseData.WriteStruct(libraryAppletInfo);

            return ResultCode.Success;
        }
        [CommandCmif(12)]
        // GetMainAppletIdentityInfo() -> nn::am::service::AppletIdentityInfo
        public ResultCode GetMainAppletIdentityInfo(ServiceCtx context)
        {
            AppletIdentifyInfo appletIdentifyInfo = new()
            {
                AppletId = AppletId.MyPage,
                TitleId = 0x0100000000001000, // Example TitleId
            };

            context.ResponseData.WriteStruct(appletIdentifyInfo);

            return ResultCode.Success;
        }
        [CommandCmif(13)]
        // CanUseApplicationCore() -> bool
        public ResultCode CanUseApplicationCore(ServiceCtx context)
        {
            context.ResponseData.Write(true); // Stub: Application Core is always available
            return ResultCode.Success;
        }

        [CommandCmif(14)]
        // GetCallerAppletIdentityInfo() -> nn::am::service::AppletIdentityInfo
        public ResultCode GetCallerAppletIdentityInfo(ServiceCtx context)
        {
            AppletIdentifyInfo appletIdentifyInfo = new()
            {
                AppletId = AppletId.QLaunch,
                TitleId = 0x0100000000001000,
            };

            context.ResponseData.WriteStruct(appletIdentifyInfo);

            return ResultCode.Success;
        }

        [CommandCmif(16)]
        // GetMainAppletStorageId() -> u32
        public ResultCode GetMainAppletStorageId(ServiceCtx context)
        {
            context.ResponseData.Write(0); // Stub: returning 0 as a placeholder
            return ResultCode.Success;
        }

        [CommandCmif(17)]
        // GetCallerAppletIdentityInfoStack() -> buffer
        public ResultCode GetCallerAppletIdentityInfoStack(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }
        [CommandCmif(18)]
        // GetNextReturnDestinationAppletIdentityInfo() -> nn::am::service::AppletIdentityInfo
        public ResultCode GetNextReturnDestinationAppletIdentityInfo(ServiceCtx context)
        {
            AppletIdentifyInfo nextDestinationInfo = new()
            {
                AppletId = AppletId.QLaunch,
                TitleId = 0x0100000000001000, // Example TitleId
            };

            context.ResponseData.WriteStruct(nextDestinationInfo);

            return ResultCode.Success;
        }

        [CommandCmif(19)]
        // GetDesirableKeyboardLayout() -> u32
        public ResultCode GetDesirableKeyboardLayout(ServiceCtx context)
        {
            context.ResponseData.Write(0); // Stub: returning 0 as a placeholder
            return ResultCode.Success;
        }
        [CommandCmif(20)]
        // PopExtraStorage() -> object<nn::am::service::IStorage>
        public ResultCode PopExtraStorage(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }


        [CommandCmif(25)]
        // GetPopExtraStorageEvent() -> handle<copy>
        public ResultCode GetPopExtraStorageEvent(ServiceCtx context)
        {
            // Stub implementation for event handle
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(0);
            return ResultCode.Success;
        }
        [CommandCmif(30)]
        // UnpopInData()
        public ResultCode UnpopInData(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }
        [CommandCmif(31)]
        // UnpopExtraStorage()
        public ResultCode UnpopExtraStorage(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(40)]
        // GetIndirectLayerProducerHandle() -> handle<copy>
        public ResultCode GetIndirectLayerProducerHandle(ServiceCtx context)
        {
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(0);
            return ResultCode.Success;
        }

        [CommandCmif(50)]
        // ReportVisibleError(u32)
        public ResultCode ReportVisibleError(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }
        [CommandCmif(60)]
        // GetMainAppletApplicationDesiredLanguage() -> u32
        public ResultCode GetMainAppletApplicationDesiredLanguage(ServiceCtx context)
        {
            context.ResponseData.Write(0); // Stub: returning default language ID
            return ResultCode.Success;
        }

        [CommandCmif(70)]
        // GetCurrentApplicationId() -> u64
        public ResultCode GetCurrentApplicationId(ServiceCtx context)
        {
            context.ResponseData.Write(0x0100000000001000); // Example ApplicationId
            return ResultCode.Success;
        }

        [CommandCmif(80)]
        // RequestExitToSelf()
        public ResultCode RequestExitToSelf(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(90)]
        // CreateApplicationAndPushAndRequestToLaunch()
        public ResultCode CreateApplicationAndPushAndRequestToLaunch(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(100)]
        // CreateGameMovieTrimmer()
        public ResultCode CreateGameMovieTrimmer(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(101)]
        // ReserveResourceForMovieOperation()
        public ResultCode ReserveResourceForMovieOperation(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(102)]
        // UnreserveResourceForMovieOperation()
        public ResultCode UnreserveResourceForMovieOperation(ServiceCtx context)
        {
            // Stub implementation
            return ResultCode.Success;
        }

        [CommandCmif(103)]
        // GetReservedResourceAmountForMovieOperation() -> u32
        public ResultCode GetReservedResourceAmountForMovieOperation(ServiceCtx context)
        {
            context.ResponseData.Write(0); // Stub: returning 0 as a placeholder
            return ResultCode.Success;
        }
        [CommandCmif(110)]
        // GetMainAppletAvailableUsers() -> 
        public ResultCode GetMainAppletAvailableUsers(ServiceCtx context)
        {
            Console.WriteLine("GetMainAppletAvailableUsers called");
            return ResultCode.Success;
        }

        [CommandCmif(120)]
        // GetLaunchStorageInfoForDebug() -> 
        public ResultCode GetLaunchStorageInfoForDebug(ServiceCtx context)
        {
            Console.WriteLine("GetLaunchStorageInfoForDebug called");
            return ResultCode.Success;
        }

        [CommandCmif(130)]
        // GetGpuErrorDetectedSystemEvent() -> 
        public ResultCode GetGpuErrorDetectedSystemEvent(ServiceCtx context)
        {
            Console.WriteLine("GetGpuErrorDetectedSystemEvent called");
            return ResultCode.Success;
        }

        [CommandCmif(140)]
        // SetApplicationMemoryReservation() ->
        public ResultCode SetApplicationMemoryReservation(ServiceCtx context)
        {
            Console.WriteLine("SetApplicationMemoryReservation called");
            return ResultCode.Success;
        }

        [CommandCmif(150)]
        // ShouldSetGpuTimeSliceManually() -> 
        public ResultCode ShouldSetGpuTimeSliceManually(ServiceCtx context)
        {
            context.ResponseData.Write(false);
            return ResultCode.Success;
        }

        [CommandCmif(160)]
        // GetLibraryAppletInfoEx() -> nn::am::service::LibraryAppletInfoEx
        public ResultCode GetLibraryAppletInfoEx(ServiceCtx context)
        {
            // Stub implementation
            LibraryAppletInfo infoEx = new()
            {
                AppletId = _appletStandalone.AppletId,
                LibraryAppletMode = _appletStandalone.LibraryAppletMode
            };

            context.ResponseData.WriteStruct(infoEx);
            return ResultCode.Success;
        }
    }
}
