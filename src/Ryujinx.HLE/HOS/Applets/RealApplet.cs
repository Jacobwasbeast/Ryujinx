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
        private readonly int _appletId;
        private readonly Horizon _system;
        private AppletSession _normalSession;
        private AppletSession _interactiveSession;
        
        public event EventHandler AppletStateChanged;
        
        public RealApplet(int appletId, Horizon system)
        {
            _system = system;
            this._appletId = appletId;
        }
        
        public ResultCode Start(AppletSession normalSession, AppletSession interactiveSession)
        {
            AppletId appletId = (AppletId)_appletId;
            string appletName = appletId.ToString();
            Console.WriteLine($"Applet {_appletId}:{appletName} started.");
            _normalSession = normalSession;
            _interactiveSession = interactiveSession;
            while (_normalSession.Length != 0)
            {
                Console.WriteLine($"Normal session data length: {_normalSession.Length}");
                _system.Device._normalDataQueue.Enqueue(_normalSession.Pop());
            }
            while (_interactiveSession.Length != 0)
            {
                Console.WriteLine($"Interactive session data length: {_interactiveSession.Length}");
                _system.Device._interactiveDataQueue.Enqueue(_interactiveSession.Pop());
            }
            _system.Device.UIHandler.StartApplet(_appletId, appletName);
            
            while ( _system.Device.UIHandler.IsAppletRunning())
            {
                
            }
            Console.WriteLine($"Applet {_appletId}:{appletName} stopped.");
            _system.ReturnFocus();
            return ResultCode.Success;
        }
        
        private static byte[] BuildResponse()
        {
            using MemoryStream stream = MemoryStreamManager.Shared.GetStream();
            using BinaryWriter writer = new(stream);
            writer.Write((ulong)ResultCode.Success);
            return stream.ToArray();
        }
    }
}
