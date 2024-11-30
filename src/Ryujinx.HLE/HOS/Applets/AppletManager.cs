using Ryujinx.Common.Configuration;
using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Applets.Browser;
using Ryujinx.HLE.HOS.Applets.Cabinet;
using Ryujinx.HLE.HOS.Applets.Dummy;
using Ryujinx.HLE.HOS.Applets.Error;
using Ryujinx.HLE.HOS.Services.Am.AppletAE;
using System;
using System.Collections.Generic;

namespace Ryujinx.HLE.HOS.Applets
{
    static class AppletManager
    {
        public static IApplet Create(AppletId applet, Horizon system)
        {
            long id = 0;
            switch (applet)
            {
                case AppletId.Controller:
                    id = 0x010000000000100C;
                    break;
                case AppletId.Error:
                    id = 0x0100000000001005;
                    break;
                case AppletId.PlayerSelect:
                    id = 0x0100000000001007;
                    break;
                case AppletId.SoftwareKeyboard:
                    id = 0x0100000000001008;
                    break;
                case AppletId.LibAppletWeb:
                    id = 0x010000000000100A;
                    break;
                case AppletId.LibAppletShop:
                    id = 0x010000000000100B;
                    break;
                case AppletId.LibAppletOff:
                    id = 0x010000000000100F;
                    break;
                case AppletId.MiiEdit:
                    id = 0x0100000000001009;
                    break;
                case AppletId.Cabinet:
                    id = 0x0100000000001002;
                    break;
            }
            if (id == 0)
            {
                Logger.Warning?.Print(LogClass.Application, $"Applet {applet} not implemented!");
                return new DummyApplet(system);
            }
            string contentPath = system.ContentManager.GetInstalledContentPath((ulong)id, LibHac.Ncm.StorageId.BuiltInSystem, LibHac.Tools.FsSystem.NcaUtils.NcaContentType.Program);
            AppDataManager.contentPath = contentPath;
            AppDataManager.id = id;
            AppDataManager.applet = applet.ToString();

            return new RealApplet(system);
        }
    }
}
