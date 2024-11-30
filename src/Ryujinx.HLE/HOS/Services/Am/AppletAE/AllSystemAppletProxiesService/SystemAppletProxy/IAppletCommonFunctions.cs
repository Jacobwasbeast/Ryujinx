using Ryujinx.HLE.HOS.Services;
using System;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE.AllSystemAppletProxiesService.SystemAppletProxy
{
    class IAppletCommonFunctions : IpcService
    {
        public IAppletCommonFunctions() { }

        [CommandCmif(0)] // [9.0.0+] #SetTerminateResult
        public void SetTerminateResult(ServiceCtx context)
        {
            Console.WriteLine("SetTerminateResult called but not implemented.");
        }

        [CommandCmif(10)] // #ReadThemeStorage
        public void ReadThemeStorage(ServiceCtx context)
        {
            Console.WriteLine("ReadThemeStorage called but not implemented.");
        }

        [CommandCmif(11)] // #WriteThemeStorage
        public void WriteThemeStorage(ServiceCtx context)
        {
            Console.WriteLine("WriteThemeStorage called but not implemented.");
        }

        [CommandCmif(20)] // [9.0.0+] #PushToAppletBoundChannel
        public void PushToAppletBoundChannel(ServiceCtx context)
        {
            Console.WriteLine("PushToAppletBoundChannel called but not implemented.");
        }

        [CommandCmif(21)] // [9.0.0+] #TryPopFromAppletBoundChannel
        public void TryPopFromAppletBoundChannel(ServiceCtx context)
        {
            Console.WriteLine("TryPopFromAppletBoundChannel called but not implemented.");
        }

        [CommandCmif(40)] // [8.0.0+] #GetDisplayLogicalResolution
        public void GetDisplayLogicalResolution(ServiceCtx context)
        {
            Console.WriteLine("GetDisplayLogicalResolution called but not implemented.");
        }

        [CommandCmif(42)] // [8.0.0+] #SetDisplayMagnification
        public void SetDisplayMagnification(ServiceCtx context)
        {
            Console.WriteLine("SetDisplayMagnification called but not implemented.");
        }

        [CommandCmif(50)] // [8.0.0+] #SetHomeButtonDoubleClickEnabled
        public void SetHomeButtonDoubleClickEnabled(ServiceCtx context)
        {
            Console.WriteLine("SetHomeButtonDoubleClickEnabled called but not implemented.");
        }

        [CommandCmif(51)] // [8.0.0+] #GetHomeButtonDoubleClickEnabled
        public void GetHomeButtonDoubleClickEnabled(ServiceCtx context)
        {
            Console.WriteLine("GetHomeButtonDoubleClickEnabled called but not implemented.");
        }

        [CommandCmif(52)] // [10.0.0+] #IsHomeButtonShortPressedBlocked
        public void IsHomeButtonShortPressedBlocked(ServiceCtx context)
        {
            Console.WriteLine("IsHomeButtonShortPressedBlocked called but not implemented.");
        }

        [CommandCmif(60)] // [11.0.0+] #IsVrModeCurtainRequired
        public void IsVrModeCurtainRequired(ServiceCtx context)
        {
            Console.WriteLine("IsVrModeCurtainRequired called but not implemented.");
        }

        [CommandCmif(61)] // [12.0.0+] #IsSleepRequiredByHighTemperature
        public void IsSleepRequiredByHighTemperature(ServiceCtx context)
        {
            Console.WriteLine("IsSleepRequiredByHighTemperature called but not implemented.");
        }

        [CommandCmif(62)] // [12.0.0+] #IsSleepRequiredByLowBattery
        public void IsSleepRequiredByLowBattery(ServiceCtx context)
        {
            Console.WriteLine("IsSleepRequiredByLowBattery called but not implemented.");
        }

        [CommandCmif(70)] // [11.0.0+] #SetCpuBoostRequestPriority
        public ResultCode SetCpuBoostRequestPriority(ServiceCtx context)
        {
            Console.WriteLine("SetCpuBoostRequestPriority called but not implemented.");
            return ResultCode.Success;
        }

        [CommandCmif(80)] // [14.0.0+] #SetHandlingCaptureButtonShortPressedMessageEnabledForApplet
        public void SetHandlingCaptureButtonShortPressedMessageEnabledForApplet(ServiceCtx context)
        {
            Console.WriteLine("SetHandlingCaptureButtonShortPressedMessageEnabledForApplet called but not implemented.");
        }

        [CommandCmif(81)] // [14.0.0+] #SetHandlingCaptureButtonLongPressedMessageEnabledForApplet
        public void SetHandlingCaptureButtonLongPressedMessageEnabledForApplet(ServiceCtx context)
        {
            Console.WriteLine("SetHandlingCaptureButtonLongPressedMessageEnabledForApplet called but not implemented.");
        }

        [CommandCmif(82)] // [18.0.0+] #SetBlockingCaptureButtonInEntireSystem
        public void SetBlockingCaptureButtonInEntireSystem(ServiceCtx context)
        {
            Console.WriteLine("SetBlockingCaptureButtonInEntireSystem called but not implemented.");
        }

        [CommandCmif(90)] // [15.0.0+] #OpenNamedChannelAsParent
        public void OpenNamedChannelAsParent(ServiceCtx context)
        {
            Console.WriteLine("OpenNamedChannelAsParent called but not implemented.");
        }

        [CommandCmif(91)] // [15.0.0+] #OpenNamedChannelAsChild
        public void OpenNamedChannelAsChild(ServiceCtx context)
        {
            Console.WriteLine("OpenNamedChannelAsChild called but not implemented.");
        }

        [CommandCmif(100)] // [15.0.0+] #SetApplicationCoreUsageMode
        public void SetApplicationCoreUsageMode(ServiceCtx context)
        {
            Console.WriteLine("SetApplicationCoreUsageMode called but not implemented.");
        }

        [CommandCmif(160)] // [18.0.0+] #GetNotificationReceiverService
        public void GetNotificationReceiverService(ServiceCtx context)
        {
            Console.WriteLine("GetNotificationReceiverService called but not implemented.");
        }

        [CommandCmif(161)] // [18.0.0+] #GetNotificationSenderService
        public void GetNotificationSenderService(ServiceCtx context)
        {
            Console.WriteLine("GetNotificationSenderService called but not implemented.");
        }

        [CommandCmif(300)] // [17.0.0+] #GetCurrentApplicationId
        public void GetCurrentApplicationId(ServiceCtx context)
        {
            Console.WriteLine("GetCurrentApplicationId called but not implemented.");
        }

        [CommandCmif(310)] // [19.0.0+] #IsSystemAppletHomeMenu
        public void IsSystemAppletHomeMenu(ServiceCtx context)
        {
            Console.WriteLine("IsSystemAppletHomeMenu called but not implemented.");
        }

        [CommandCmif(320)] // [19.0.0+] #SetGpuTimeSliceBoost
        public void SetGpuTimeSliceBoost(ServiceCtx context)
        {
            Console.WriteLine("SetGpuTimeSliceBoost called but not implemented.");
        }

        [CommandCmif(321)] // [19.0.0+] #SetGpuTimeSliceBoostDueToApplication
        public void SetGpuTimeSliceBoostDueToApplication(ServiceCtx context)
        {
            Console.WriteLine("SetGpuTimeSliceBoostDueToApplication called but not implemented.");
        }

        [CommandCmif(330)] // [19.0.0+] Undefined Command
        public void UndefinedCommand(ServiceCtx context)
        {
            Console.WriteLine("UndefinedCommand called but not implemented.");
        }
    }
}
