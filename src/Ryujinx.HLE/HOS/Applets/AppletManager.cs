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
            int appId = (int)applet;
            string appName = Enum.GetName(typeof(AppletId), applet);
            Console.WriteLine($"Creating applet {appName} ({appId})");
            return new RealApplet(appId, system);
        }
    }
}
