using Ryujinx.HLE.HOS.Services.Sm;
using Ryujinx.Horizon;

namespace Ryujinx.HLE.HOS.Services
{
    public class ServerBaseManager
    {
        internal SmRegistry SmRegistry { get; set; }
        internal ServerBase SmServer { get; set; }
        internal ServerBase BsdServer { get; set; }
        internal ServerBase FsServer { get; set; }
        internal ServerBase HidServer { get; set; }
        internal ServerBase NvDrvServer { get; set; }
        internal ServerBase TimeServer { get; set; }
        internal ServerBase ViServer { get; set; }
        internal ServerBase ViServerM { get; set; }
        internal ServerBase ViServerS { get; set; }
        internal ServerBase LdnServer { get; set; }
        internal ServiceTable ServiceTable { get; set; }
    }
}
