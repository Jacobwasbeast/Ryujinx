using LibHac.Ns;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Utilities;
using Ryujinx.HLE.HOS.Ipc;
using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.Horizon.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ryujinx.HLE.HOS.Services.Ns
{
    [Service("ns:am")]
    class IApplicationManagerInterface : IpcService
    {
        KEvent _applicationRecordUpdateEvent;
        int _applicationRecordUpdateEventHandle;
        KEvent _sdCardMountStatusChangedEvent;
        int _sdCardMountStatusChangedEventHandle;
        KEvent _gameCardUpdateDetectionEvent;
        int _gameCardUpdateDetectionEventHandle;
        KEvent _gameCardMountFailureEvent;
        int _gameCardMountFailureEventHandle;

        public IApplicationManagerInterface(ServiceCtx context)
        {
            _applicationRecordUpdateEvent = new KEvent(context.Device.System.KernelContext);
            _applicationRecordUpdateEventHandle = -1;
            _sdCardMountStatusChangedEvent = new KEvent(context.Device.System.KernelContext);
            _sdCardMountStatusChangedEventHandle = -1;
            _gameCardUpdateDetectionEvent = new KEvent(context.Device.System.KernelContext);
            _gameCardUpdateDetectionEventHandle = -1;
            _gameCardMountFailureEvent = new KEvent(context.Device.System.KernelContext);
            _gameCardMountFailureEventHandle = -1;
        }

        [CommandCmif(0)]
        // ListApplicationRecord(unknown<4>) -> (unknown<4>, buffer<unknown, 6>)
        // unknown<4> is entry_offset and buffer<unknown, 6> is  OutArray<ApplicationRecord, BufferAttr_HipcMapAlias> out_records
        public ResultCode ListApplicationRecord(ServiceCtx context)
        {
            // Read entry_offset from the input data
            int entryOffset = context.RequestData.ReadInt32();
            Logger.Info?.PrintStub(LogClass.ServiceNs, $"ListApplicationRecord: Entry offset {entryOffset}");

            var outputBuffer = context.Request.ReceiveBuff[0];
            ulong bufferCapacity = outputBuffer.Size / 0x18;
            Logger.Info?.PrintStub(LogClass.ServiceNs, $"ListApplicationRecord: Output buffer capacity {bufferCapacity}");

            // Simulate fetching installed games (replace with actual data retrieval logic)
            var installedGames = context.Device.UIHandler.GetApplications();
            Logger.Info?.PrintStub(LogClass.ServiceNs, $"ListApplicationRecord: {installedGames.Count} installed games found");

            int recordCount = 0;
            int index = 0;
            int ii = 24;

            // Populate the output buffer with ApplicationRecords
            foreach (var game in installedGames)
            {
                // if (recordCount >= bufferCapacity)
                // {
                //     Logger.Info?.PrintStub(LogClass.ServiceNs, "ListApplicationRecord: Output buffer full");
                //     break;
                // }
                
                if (index < entryOffset)
                {
                    Logger.Info?.PrintStub(LogClass.ServiceNs, $"ListApplicationRecord: Skipping record {index}");
                    index++;
                    continue;
                }

                ApplicationRecord record = new ApplicationRecord
                {
                    ApplicationId = game.Value,
                    Type = (byte)ApplicationRecordType.Installed,
                    Unknown1 = 0, // Placeholder
                    Unknown3 = (byte)ii++
                };
                // TODO: Populate with actual data
                var positon = outputBuffer.Position + (ulong)recordCount * 0x18;
                context.Memory.Write(positon, SpanHelpers.AsByteSpan(ref record).ToArray());
                
                recordCount++;
            }
            Logger.Info?.PrintStub(LogClass.ServiceNs, $"ListApplicationRecord: {recordCount} records written");

            // Write the output entry count
            context.ResponseData.Write(recordCount);
            return ResultCode.Success;
        }

        [CommandCmif(2)]
        // GetApplicationRecordUpdateSystemEvent() -> handle<copy>
        public ResultCode GetApplicationRecordUpdateSystemEvent(ServiceCtx context)
        {
            if (_applicationRecordUpdateEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_applicationRecordUpdateEvent.ReadableEvent, out _applicationRecordUpdateEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }

            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_applicationRecordUpdateEventHandle);

            Logger.Stub?.PrintStub(LogClass.Service);
            return ResultCode.Success;
        }

        [CommandCmif(44)]
        // GetSdCardMountStatusChangedEvent() -> handle<copy>
        public ResultCode GetSdCardMountStatusChangedEvent(ServiceCtx context)
        {
            if (_sdCardMountStatusChangedEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_sdCardMountStatusChangedEvent.ReadableEvent, out _sdCardMountStatusChangedEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }
            
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_sdCardMountStatusChangedEventHandle);
            
            Logger.Stub?.PrintStub(LogClass.Service);
            return ResultCode.Success;
        }
        
        [CommandCmif(52)]
        // GetGameCardUpdateDetectionEvent() -> handle<copy>
        public ResultCode GetGameCardUpdateDetectionEvent(ServiceCtx context)
        {
            if (_gameCardUpdateDetectionEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_gameCardUpdateDetectionEvent.ReadableEvent, out _gameCardUpdateDetectionEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }
            
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_gameCardUpdateDetectionEventHandle);
            
            Logger.Stub?.PrintStub(LogClass.Service);
            return ResultCode.Success;
        }
        
        [CommandCmif(400)]
        // GetApplicationControlData(u8, u64) -> (unknown<4>, buffer<unknown, 6>)
        public ResultCode GetApplicationControlData(ServiceCtx context)
        {
#pragma warning disable IDE0059 // Remove unnecessary value assignment
            byte source = (byte)context.RequestData.ReadInt64();
            ulong titleId = context.RequestData.ReadUInt64();
#pragma warning restore IDE0059

            ulong position = context.Request.ReceiveBuff[0].Position;

            ApplicationControlProperty nacp = context.Device.Processes.ActiveApplication.ApplicationControlProperties;

            context.Memory.Write(position, SpanHelpers.AsByteSpan(ref nacp).ToArray());

            return ResultCode.Success;
        }
        
        [CommandCmif(505)]
        // GetGameCarudMontFailureEvent() -> handle<copy>
        public ResultCode GetGameCardMountFailureEvent(ServiceCtx context)
        {
            if (_gameCardMountFailureEventHandle == -1)
            {
                Result resultCode = context.Process.HandleTable.GenerateHandle(_gameCardMountFailureEvent.ReadableEvent, out _gameCardMountFailureEventHandle);

                if (resultCode != Result.Success)
                {
                    return (ResultCode)resultCode.ErrorCode;
                }
            }
            
            context.Response.HandleDesc = IpcHandleDesc.MakeCopy(_gameCardMountFailureEventHandle);
            
            Logger.Stub?.PrintStub(LogClass.Service);
            return ResultCode.Success;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, Size = 0x18, Pack = 1)]
    unsafe public struct ApplicationRecord
    {
        public ulong ApplicationId;
        public byte Type;
        public byte Unknown1;
        // Replace arrays with fixed-size buffers
        private fixed byte Unknown2[6];
        public byte Unknown3;
        private fixed byte Unknown4[7];

        // Helper properties to access fixed-size buffers as arrays
        public Span<byte> Unknown2Span
        {
            get
            {
                unsafe
                {
                    fixed (byte* ptr = Unknown2)
                    {
                        return new Span<byte>(ptr, 6);
                    }
                }
            }
        }

        public Span<byte> Unknown4Span
        {
            get
            {
                unsafe
                {
                    fixed (byte* ptr = Unknown4)
                    {
                        return new Span<byte>(ptr, 7);
                    }
                }
            }
        }
    }
    
    public enum ApplicationRecordType
    {
        Installed = 0x3,
    }
}
