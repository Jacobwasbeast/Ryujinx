using LibHac.Ncm;
using LibHac.Tools.FsSystem.NcaUtils;
using Ryujinx.Common.Logging;
using Ryujinx.Common.Memory;
using Ryujinx.HLE.FileSystem;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Ryujinx.HLE.HOS.Services.Nfc.Nfp.NfpManager;
using System.Runtime.CompilerServices;
using static LibHac.FsSrv.Impl.AccessControlBits;
using Ryujinx.Graphics.Gpu;
using Ryujinx.Common.Configuration;
using Ryujinx.HLE.HOS.Services.Hid;
using Ryujinx.HLE.HOS.Services.Hid.HidServer;
using System.Threading.Tasks;
using System.Threading;
using Ryujinx.HLE.HOS.Services.Nfc.Nfp;
namespace Ryujinx.HLE.HOS.Applets.Cabinet
{
    internal class CabinetApplet : IApplet
    {
        private readonly Horizon _system;
        private AppletSession _normalSession;
        private AppletSession _interactiveSession;
        public event EventHandler AppletStateChanged;
        public CabinetApplet(Horizon system)
        {
            _system = system;
        }
        public ResultCode Start(AppletSession normalSession, AppletSession interactiveSession)
        {
            _normalSession = normalSession;
            _interactiveSession = interactiveSession;

            return ResultCode.Success;
        }

        public ResultCode GetResult()
        {
            return ResultCode.Success;
        }
    }
}
