using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ryujinx.HLE.HOS.Applets.Real
{
    public class RealAppletManager
    {
        public Horizon Horizon { get; }
        
        public Dictionary<ulong, RealApplet> RegisteredApplets { get; } = new Dictionary<ulong, RealApplet>();
        public RealAppletManager(Horizon horizon)
        {
            Horizon = horizon;
        }
        
        public bool HasAppletId(ulong id) => RegisteredApplets.ContainsKey(id);

        public ConcurrentQueue<byte[]> GetAppletInputData(ulong appletId)
        {
            return RegisteredApplets[appletId].InputBuffer;
        }
        
        public void RegisterApplet(ulong id, RealApplet app)
        {
            RegisteredApplets.Add(id, app);
        }
        
        public RealApplet GetApplet(ulong appletId) => RegisteredApplets[appletId];

        public void RemoveApplet(ulong programId)
        {
            RegisteredApplets.Remove(programId);
        }

        public bool IsRealApplet(ulong programId)
        {
            // TODO: This is a temporary solution. We need to actually implement settings for this.
            //       For now we just check if the applet is in our list of known real applets.
            switch (programId)
            {
                case 0x0100000000001009: // MiiEdit
                case 0x0100000000001002: // Cabinet
                case 0x0100000000001003: // Controller
                case 0x0100000000001005: // Error    
                case 0x0100000000001007: // PlayerSelect    
                case 0x0100000000001008: // SoftwareKeyboard
                    return true;
                default:
                    return false;    
            }
        }
    }
}
