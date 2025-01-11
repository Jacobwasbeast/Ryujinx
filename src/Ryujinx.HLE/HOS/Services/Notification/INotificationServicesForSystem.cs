using Ryujinx.Common;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using System;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Services.Notification
{
    [Service("notif:s")] // 9.0.0+
    class INotificationServicesForSystem : IpcService
    {
        public INotificationServicesForSystem(ServiceCtx context) { }

        [CommandCmif(520)]
        // ListAlarmSettings() -> s32, span<nn::ns::detail::AlarmSetting>
        public ResultCode ListAlarmSettings(ServiceCtx context)
        {
            ulong bufferPosition = context.Request.ReceiveBuff[0].Position;
            AlarmSetting alarmSetting = AlarmSetting.InitializeDefault();
            Span<AlarmSetting> alarmSettings = MemoryMarshal.CreateSpan(ref alarmSetting, 1);
            ReadOnlySpan<byte> alarmSettingBytes = MemoryMarshal.AsBytes(alarmSettings);
            context.ResponseData.Write(alarmSettings.Length);
            context.Memory.Write(bufferPosition, alarmSettingBytes);
            return ResultCode.Success;
        }
        
        [StructLayout(LayoutKind.Sequential, Size = 0x40, Pack = 1)]
        public struct AlarmSetting
        {
            // 0x00: AlarmSettingId (0x02 bytes)
            public ushort AlarmSettingId;

            // 0x02: Kind (0x01 byte)
            public byte Kind;

            // 0x03: Muted (0x01 byte)
            public byte Muted;

            // 0x04: Padding (0x04 bytes)
            public Array4<byte> Padding; 

            // 0x08: UID (0x10 bytes)
            public Array16<byte> UID;

            // 0x18: ApplicationId (0x08 bytes)
            public ulong ApplicationId;

            // 0x20: Not set by sdksno, besides clearing it during initialization (0x08 bytes)
            public ulong NotSet;

            // 0x28: Alarm schedule (0x18 bytes) - WeeklyScheduleAlarmSetting
            public Array24<byte> Schedule;

            // Helper method to initialize the struct to default values
            public static AlarmSetting InitializeDefault()
            {
                var setting = new AlarmSetting
                {
                    Padding = new Ryujinx.Common.Memory.Array4<byte>(),
                    UID = new Ryujinx.Common.Memory.Array16<byte>(),
                    Schedule = new Ryujinx.Common.Memory.Array24<byte>()
                };

                // Set the last 0xE bytes of the Schedule to 0xFF
                for (int i = 0xA; i < 0x18; i++)
                {
                    setting.Schedule[i] = 0xFF;
                }

                return setting;
            }
        }

        [CommandCmif(1040)]
        // GetNotificationSendingNotifier() -> nn::ns::detail::INotificationSystemEventAccessor
        public ResultCode GetNotificationSendingNotifier(ServiceCtx context)
        {
            MakeObject(context, new INotificationSystemEventAccessor(context));

            return ResultCode.Success;
        }
        
        [CommandCmif(1510)]
        // GetNotificationPresentationSetting() -> nn::ns::detail::NotificationPresentationSetting
        public ResultCode GetNotificationPresentationSetting(ServiceCtx context)
        {
            context.ResponseData.WriteStruct(new NotificationPresentationSetting());
            Logger.Stub?.PrintStub(LogClass.ServiceSet, "Stubbed.");
            
            return ResultCode.Success;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NotificationPresentationSetting
        {
            // 16 bytes of padding
            public Array10<byte> Padding;

            // Constructor to initialize the padding if needed
            public NotificationPresentationSetting()
            {
                Padding = new Array10<byte>();
            }
        }
    }
}
