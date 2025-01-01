using Ryujinx.Common;
using System;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.LibraryAppletProxy
{
    class ILibraryAppletSelfAccessor : IpcService
    {
        private readonly AppletStandalone _appletStandalone = new();

        public ILibraryAppletSelfAccessor(ServiceCtx context)
        {
            /*if (context.Device.Processes.ActiveApplication.ProgramId == 0x0100000000001009)
            {
                // Create MiiEdit data.
                _appletStandalone = new AppletStandalone()
                {
                    AppletId = AppletId.MiiEdit,
                    LibraryAppletMode = LibraryAppletMode.AllForeground,
                };

                byte[] miiEditInputData = new byte[0x100];
                miiEditInputData[0] = 0x03; // Hardcoded unknown value.

                _appletStandalone.InputData.Enqueue(miiEditInputData);
            }
            else
            {
                
            }*/
            ulong id = context.Device.Processes.ActiveApplication.ProgramId;
            AppletId name = AppletId.Application;
            switch (id)
            {
                case (0x0100000000001009) :
                    name = AppletId.MiiEdit;
                    break;
            }
            _appletStandalone = new AppletStandalone()
            {
                AppletId = name,
                LibraryAppletMode = LibraryAppletMode.AllForeground,
            };
            if (!context.Device._normalDataQueue.IsEmpty)
            {
                foreach (byte[] data in context.Device._normalDataQueue)
                {
                    _appletStandalone.InputData.Enqueue(data);
                    Console.WriteLine($"Added data to input queue: {data}");
                }
            }
            else
            {
                if (context.Device.Processes.ActiveApplication.ProgramId == 0x0100000000001009)
                {
                    // Create MiiEdit data.
                    _appletStandalone = new AppletStandalone()
                    {
                        AppletId = AppletId.MiiEdit,
                        LibraryAppletMode = LibraryAppletMode.AllForeground,
                    };

                    byte[] miiEditInputData = new byte[0x100];
                    miiEditInputData[0] = 0x03; // Hardcoded unknown value.

                    _appletStandalone.InputData.Enqueue(miiEditInputData);
                }
            }
        }

        [CommandCmif(0)]
        // PopInData() -> object<nn::am::service::IStorage>
        public ResultCode PopInData(ServiceCtx context)
        {
            byte[] appletData = _appletStandalone.InputData.Dequeue();

            if (appletData.Length == 0)
            {
                return ResultCode.NotAvailable;
            }

            MakeObject(context, new IStorage(appletData));

            return ResultCode.Success;
        }
        
        [CommandCmif(1)]
        // IsCompleted() ->  returns an output u8 bool. 
        public ResultCode IsCompleted(ServiceCtx context)
        {
            context.ResponseData.Write(1);

            return ResultCode.Success;
        }
        
        [CommandCmif(10)]
        // Start()
        public ResultCode Start(ServiceCtx context)
        {
            context.Device.UIHandler.StopApplet();
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
    }
}
