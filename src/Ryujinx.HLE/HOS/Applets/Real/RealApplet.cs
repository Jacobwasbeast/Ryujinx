using Ryujinx.HLE.HOS.Kernel.Threading;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using Ryujinx.HLE.HOS.SystemState;
using System.Collections.Concurrent;

namespace Ryujinx.HLE.HOS.Applets.Real
{
    public class RealApplet
    {
        internal KEvent _stateChangedEvent;
        public ulong BeforeId { get; set; }
        public ConcurrentQueue<byte[]> InputBuffer { get; } = new();
        public ConcurrentQueue<byte[]> InteractiveBuffer { get; } = new();
        internal AppletId AppletId { get; set; }
        internal LibraryAppletMode LibraryAppletMode { get; set; }
        
        internal AppletStateMgr AppletManagerBefore { get; set; }
        public long LastRenderLayer { get; set; }
        public long AppRenderLayer { get; set; }
        public bool IsRunning { get; set; }
    }
}
