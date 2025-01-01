using Ryujinx.Common.Logging;
namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class IAppletCommonFunctions : IpcService
    {
        public IAppletCommonFunctions() { }

        // [9.0.0+] #SetTerminateResult
        [CommandCmif(0)]
        public ResultCode SetTerminateResult(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetTerminateResult called");
            return ResultCode.Success;
        }

        // #ReadThemeStorage
        [CommandCmif(10)]
        public ResultCode ReadThemeStorage(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "ReadThemeStorage called");
            // Placeholder for reading theme storage
            return ResultCode.Success;
        }

        // #WriteThemeStorage
        [CommandCmif(11)]
        public ResultCode WriteThemeStorage(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "WriteThemeStorage called");
            // Placeholder for writing theme storage
            return ResultCode.Success;
        }

        // [9.0.0+] #PushToAppletBoundChannel
        [CommandCmif(20)]
        public ResultCode PushToAppletBoundChannel(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "PushToAppletBoundChannel called");
            return ResultCode.Success;
        }

        // [9.0.0+] #TryPopFromAppletBoundChannel
        [CommandCmif(21)]
        public ResultCode TryPopFromAppletBoundChannel(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "TryPopFromAppletBoundChannel called");
            return ResultCode.Success;
        }

        // [8.0.0+] #GetDisplayLogicalResolution
        [CommandCmif(40)]
        public ResultCode GetDisplayLogicalResolution(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "GetDisplayLogicalResolution called");
            return ResultCode.Success;
        }

        // [8.0.0+] #SetDisplayMagnification
        [CommandCmif(42)]
        public ResultCode SetDisplayMagnification(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetDisplayMagnification called");
            return ResultCode.Success;
        }

        // [8.0.0+] #SetHomeButtonDoubleClickEnabled
        [CommandCmif(50)]
        public ResultCode SetHomeButtonDoubleClickEnabled(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetHomeButtonDoubleClickEnabled called");
            return ResultCode.Success;
        }

        // [8.0.0+] #GetHomeButtonDoubleClickEnabled
        [CommandCmif(51)]
        public ResultCode GetHomeButtonDoubleClickEnabled(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "GetHomeButtonDoubleClickEnabled called");
            return ResultCode.Success;
        }

        // [10.0.0+] #IsHomeButtonShortPressedBlocked
        [CommandCmif(52)]
        public ResultCode IsHomeButtonShortPressedBlocked(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "IsHomeButtonShortPressedBlocked called");
            return ResultCode.Success;
        }

        // [11.0.0+] #IsVrModeCurtainRequired
        [CommandCmif(60)]
        public ResultCode IsVrModeCurtainRequired(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "IsVrModeCurtainRequired called");
            return ResultCode.Success;
        }

        // [12.0.0+] IsSleepRequiredByHighTemperature
        [CommandCmif(61)]
        public ResultCode IsSleepRequiredByHighTemperature(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "IsSleepRequiredByHighTemperature called");
            return ResultCode.Success;
        }

        // [12.0.0+] IsSleepRequiredByLowBattery
        [CommandCmif(62)]
        public ResultCode IsSleepRequiredByLowBattery(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "IsSleepRequiredByLowBattery called");
            return ResultCode.Success;
        }

        // [11.0.0+] #SetCpuBoostRequestPriority
        [CommandCmif(70)]
        public ResultCode SetCpuBoostRequestPriority(ServiceCtx context)
        {
            int priority = context.RequestData.ReadInt32();
            Logger.Info?.Print(LogClass.ServiceAm, $"SetCpuBoostRequestPriority called with priority: {priority}");
            return ResultCode.Success;
        }

        // [14.0.0+] SetHandlingCaptureButtonShortPressedMessageEnabledForApplet
        [CommandCmif(80)]
        public ResultCode SetHandlingCaptureButtonShortPressedMessageEnabledForApplet(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetHandlingCaptureButtonShortPressedMessageEnabledForApplet called");
            return ResultCode.Success;
        }

        // [14.0.0+] SetHandlingCaptureButtonLongPressedMessageEnabledForApplet
        [CommandCmif(81)]
        public ResultCode SetHandlingCaptureButtonLongPressedMessageEnabledForApplet(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetHandlingCaptureButtonLongPressedMessageEnabledForApplet called");
            return ResultCode.Success;
        }

        // [18.0.0+] SetBlockingCaptureButtonInEntireSystem
        [CommandCmif(82)]
        public ResultCode SetBlockingCaptureButtonInEntireSystem(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetBlockingCaptureButtonInEntireSystem called");
            return ResultCode.Success;
        }

        // [15.0.0+] OpenNamedChannelAsParent
        [CommandCmif(90)]
        public ResultCode OpenNamedChannelAsParent(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "OpenNamedChannelAsParent called");
            return ResultCode.Success;
        }

        // [15.0.0+] OpenNamedChannelAsChild
        [CommandCmif(91)]
        public ResultCode OpenNamedChannelAsChild(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "OpenNamedChannelAsChild called");
            return ResultCode.Success;
        }

        // [15.0.0+] SetApplicationCoreUsageMode
        [CommandCmif(100)]
        public ResultCode SetApplicationCoreUsageMode(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetApplicationCoreUsageMode called");
            return ResultCode.Success;
        }

        // [18.0.0+] GetNotificationReceiverService
        [CommandCmif(160)]
        public ResultCode GetNotificationReceiverService(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "GetNotificationReceiverService called");
            return ResultCode.Success;
        }

        // [18.0.0+] GetNotificationSenderService
        [CommandCmif(161)]
        public ResultCode GetNotificationSenderService(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "GetNotificationSenderService called");
            return ResultCode.Success;
        }

        // [17.0.0+] GetCurrentApplicationId
        [CommandCmif(300)]
        public ResultCode GetCurrentApplicationId(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "GetCurrentApplicationId called");
            return ResultCode.Success;
        }

        // [19.0.0+] IsSystemAppletHomeMenu
        [CommandCmif(310)]
        public ResultCode IsSystemAppletHomeMenu(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "IsSystemAppletHomeMenu called");
            return ResultCode.Success;
        }

        // [19.0.0+] SetGpuTimeSliceBoost
        [CommandCmif(320)]
        public ResultCode SetGpuTimeSliceBoost(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetGpuTimeSliceBoost called");
            return ResultCode.Success;
        }

        // [19.0.0+] SetGpuTimeSliceBoostDueToApplication
        [CommandCmif(321)]
        public ResultCode SetGpuTimeSliceBoostDueToApplication(ServiceCtx context)
        {
            Logger.Info?.Print(LogClass.ServiceAm, "SetGpuTimeSliceBoostDueToApplication called");
            return ResultCode.Success;
        }
    }
}
