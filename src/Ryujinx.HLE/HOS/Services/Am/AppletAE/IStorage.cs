using System;

namespace Ryujinx.HLE.HOS.Services.Am.AppletAE
{
    class IStorage : IpcService
    {
        public bool IsReadOnly { get; private set; }
        public byte[] Data { get; private set; }

        public IStorage(byte[] data, bool isReadOnly = false)
        {
            IsReadOnly = isReadOnly;
            Data = data;
        }

        [CommandCmif(0)]
        // Open() -> object<nn::am::service::IStorageAccessor>
        public ResultCode Open(ServiceCtx context)
        {
            MakeObject(context, new IStorageAccessor(this));

            return ResultCode.Success;
        }
        
        [CommandCmif(1)]
        // Read() -> object<nn::am::service::ITransferStorageAccessor>
        public ResultCode Read(ServiceCtx context)
        {
            ulong readPosition = context.RequestData.ReadUInt64();

            if (readPosition > (ulong)Data.Length)
            {
                return ResultCode.OutOfBounds;
            }

            (ulong position, ulong size) = context.Request.GetBufferType0x22();

            size = Math.Min(size, (ulong)Data.Length - readPosition);

            byte[] data = new byte[size];

            Buffer.BlockCopy(Data, (int)readPosition, data, 0, (int)size);

            context.Memory.Write(position, data);

            return ResultCode.Success;
        }
    }
}
