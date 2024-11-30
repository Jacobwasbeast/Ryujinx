using Ryujinx.Common.Configuration;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.IO;
using System.Runtime.InteropServices;
namespace Ryujinx.HLE.HOS.Applets
{
    internal class RealApplet : IApplet
    {
        private readonly Horizon _system;
        private AppletSession _normalSession;
        public event EventHandler AppletStateChanged;
        public RealApplet(Horizon system)
        {
            _system = system;
        }
        public ResultCode Start(AppletSession normalSession, AppletSession interactiveSession)
        {
            _normalSession = normalSession;
            AppDataManager.isAppletDone = false;
            AppDataManager.normalSession = normalSession;
            AppDataManager.inputData = new System.Collections.Concurrent.ConcurrentQueue<byte[]>(); // This is a queue of byte arrays
            while (_normalSession._inputData.Count > 0)
            {
                AppDataManager.inputData.Enqueue(_normalSession.Pop());
            }
            AppDataManager.interactiveSession = interactiveSession;
            return ResultCode.Success;
        }
        private static T ReadStruct<T>(byte[] data) where T : struct
        {
            return MemoryMarshal.Read<T>(data.AsSpan());
        }
        public ResultCode GetResult()
        {
            while (AppDataManager.isAppletDone)
            {

            }
            return ResultCode.Success;
        }
    }
}
