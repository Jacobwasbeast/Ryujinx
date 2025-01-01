using Avalonia.Controls;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using LibHac.Ncm;
using LibHac.Tools.FsSystem.NcaUtils;
using Ryujinx.Ava.Common.Locale;
using Ryujinx.Ava.UI.Controls;
using Ryujinx.Ava.UI.Helpers;
using Ryujinx.Ava.UI.ViewModels;
using Ryujinx.Ava.UI.Windows;
using Ryujinx.Ava.Utilities;
using Ryujinx.Ava.Utilities.AppLibrary;
using Ryujinx.Ava.Utilities.Configuration;
using Ryujinx.HLE;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Applets.SoftwareKeyboard;
using Ryujinx.HLE.HOS.Services.Am.AppletOE.ApplicationProxyService.ApplicationProxy.Types;
using Ryujinx.HLE.UI;
using System;
using System.Threading;

namespace Ryujinx.Ava.UI.Applet
{
    internal class AvaHostUIHandler : IHostUIHandler
    {
        private readonly MainWindow _parent;

        public IHostUITheme HostUITheme { get; }
        public void StartApplet(int appletIdInt, string appletName)
        {
            ulong appletId = 0;
            switch (appletIdInt)
            {
                // 0x02  010000000000100C  OverlayApplet (overlayDisp)
                case 0x02:
                    appletId = 0x010000000000100C;
                    break;

                // 0x03  0100000000001000  SystemAppletMenu (qlaunch)
                case 0x03:
                    appletName = "qlaunch";
                    appletId = 0x0100000000001000;
                    break;

                // 0x04  0100000000001012  SystemApplication (starter)
                case 0x04:
                    appletName = "starter";
                    appletId = 0x0100000000001012;
                    break;

                // 0x0A  0100000000001001  LibraryAppletAuth (auth)
                case 0x0A:
                    appletId = 0x0100000000001001;
                    break;

                // 0x0B  0100000000001002  LibraryAppletCabinet (cabinet)
                case 0x0B:
                    appletName = "cabinet";
                    appletId = 0x0100000000001002;
                    break;

                // 0x0C  0100000000001003  LibraryAppletController (controller)
                case 0x0C:
                    appletName = "controller";
                    appletId = 0x0100000000001003;
                    break;

                // 0x0D  0100000000001004  LibraryAppletDataErase (dataErase)
                case 0x0D:
                    appletId = 0x0100000000001004;
                    break;

                // 0x0E  0100000000001005  LibraryAppletError (error)
                case 0x0E:
                    appletId = 0x0100000000001005;
                    break;

                // 0x0F  0100000000001006  LibraryAppletNetConnect (netConnect)
                case 0x0F:
                    appletId = 0x0100000000001006;
                    break;

                // 0x10  0100000000001007  LibraryAppletPlayerSelect (playerSelect)
                case 0x10:
                    appletId = 0x0100000000001007;
                    break;

                // 0x11  0100000000001008  LibraryAppletSwkbd (swkbd)
                case 0x11:
                    appletId = 0x0100000000001008;
                    break;

                // 0x12  0100000000001009  LibraryAppletMiiEdit (miiEdit)
                case 0x12:
                    appletName = "miiEdit";
                    appletId = 0x0100000000001009;
                    break;

                // 0x13  010000000000100A  LibraryAppletWeb (web)
                case 0x13:
                    appletName = "web";
                    appletId = 0x010000000000100A;
                    break;

                // 0x14  010000000000100B  LibraryAppletShop (shop)
                case 0x14:
                    appletName = "shop";
                    appletId = 0x010000000000100B;
                    break;

                // 0x15  010000000000100D  LibraryAppletPhotoViewer (photoViewer)
                case 0x15:
                    appletId = 0x010000000000100D;
                    break;

                // 0x16  010000000000100E  LibraryAppletSet (set)
                case 0x16:
                    appletId = 0x010000000000100E;
                    break;

                // 0x17  010000000000100F  LibraryAppletOfflineWeb (offlineWeb)
                case 0x17:
                    appletId = 0x010000000000100F;
                    break;

                // 0x18  0100000000001010  LibraryAppletLoginShare (loginShare)
                case 0x18:
                    appletId = 0x0100000000001010;
                    break;

                // 0x19  0100000000001011  LibraryAppletWifiWebAuth (wifiWebAuth)
                case 0x19:
                    appletId = 0x0100000000001011;
                    break;

                // 0x1A  0100000000001013  LibraryAppletMyPage (myPage)
                case 0x1A:
                    appletId = 0x0100000000001013;
                    break;

                // 0x1B  010000000000101A  LibraryAppletGift (gift)
                case 0x1B:
                    appletId = 0x010000000000101A;
                    break;

                // 0x1C  010000000000101C  LibraryAppletUserMigration (userMigration)
                case 0x1C:
                    appletId = 0x010000000000101C;
                    break;

                // Default case to handle unexpected applet IDs
                default:
                    throw new ArgumentOutOfRangeException(nameof(appletIdInt), $"Unhandled appletIdInt: {appletIdInt}");
            }
            AppletMetadata Applet = new(appletName, appletId);
            Console.WriteLine($"Starting applet {appletName} with ID {appletId}");
            if (Applet.CanStart(_parent.ContentManager, out var appData, out var nacpData)) {
                _parent.ViewModel.LoadApplicationApplet(appData, _parent.ViewModel.IsFullScreen || _parent.ViewModel.StartGamesInFullscreen, nacpData);
            }
        }

        public void StopApplet()
        {
            _parent.ViewModel.AppHost?.Stop();
        }

        public AvaHostUIHandler(MainWindow parent)
        {
            _parent = parent;

            HostUITheme = new AvaloniaHostUITheme(parent);
        }

        public bool DisplayMessageDialog(ControllerAppletUIArgs args)
        {
            ManualResetEvent dialogCloseEvent = new(false);

            bool okPressed = false;

            if (ConfigurationState.Instance.IgnoreApplet)
                return false;

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var response = await ControllerAppletDialog.ShowControllerAppletDialog(_parent, args);
                if (response == UserResult.Ok)
                {
                    okPressed = true;
                }

                dialogCloseEvent.Set();
            });

            dialogCloseEvent.WaitOne();

            return okPressed;
        }

        public bool IsAppletRunning()
        {
            return MainWindowViewModel.AppHostApplet != null;
        }

        public bool DisplayMessageDialog(string title, string message)
        {
            ManualResetEvent dialogCloseEvent = new(false);

            bool okPressed = false;

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    ManualResetEvent deferEvent = new(false);

                    bool opened = false;

                    UserResult response = await ContentDialogHelper.ShowDeferredContentDialog(_parent,
                       title,
                       message,
                       string.Empty,
                       LocaleManager.Instance[LocaleKeys.DialogOpenSettingsWindowLabel],
                       string.Empty,
                       LocaleManager.Instance[LocaleKeys.SettingsButtonClose],
                       (int)Symbol.Important,
                       deferEvent,
                       async window =>
                       {
                           if (opened)
                           {
                               return;
                           }

                           opened = true;

                           _parent.SettingsWindow = new SettingsWindow(_parent.VirtualFileSystem, _parent.ContentManager);

                           await _parent.SettingsWindow.ShowDialog(window);

                           _parent.SettingsWindow = null;

                           opened = false;
                       });

                    if (response == UserResult.Ok)
                    {
                        okPressed = true;
                    }

                    dialogCloseEvent.Set();
                }
                catch (Exception ex)
                {
                    await ContentDialogHelper.CreateErrorDialog(LocaleManager.Instance.UpdateAndGetDynamicValue(LocaleKeys.DialogMessageDialogErrorExceptionMessage, ex));

                    dialogCloseEvent.Set();
                }
            });

            dialogCloseEvent.WaitOne();

            return okPressed;
        }

        public bool DisplayInputDialog(SoftwareKeyboardUIArgs args, out string userText)
        {
            ManualResetEvent dialogCloseEvent = new(false);

            bool okPressed = false;
            bool error = false;
            string inputText = args.InitialText ?? string.Empty;

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    _parent.ViewModel.AppHost.NpadManager.BlockInputUpdates();
                    (UserResult result, string userInput) = await SwkbdAppletDialog.ShowInputDialog(LocaleManager.Instance[LocaleKeys.SoftwareKeyboard], args);

                    if (result == UserResult.Ok)
                    {
                        inputText = userInput;
                        okPressed = true;
                    }
                }
                catch (Exception ex)
                {
                    error = true;

                    await ContentDialogHelper.CreateErrorDialog(LocaleManager.Instance.UpdateAndGetDynamicValue(LocaleKeys.DialogSoftwareKeyboardErrorExceptionMessage, ex));
                }
                finally
                {
                    dialogCloseEvent.Set();
                }
            });

            dialogCloseEvent.WaitOne();
            _parent.ViewModel.AppHost.NpadManager.UnblockInputUpdates();

            userText = error ? null : inputText;

            return error || okPressed;
        }

        public bool DisplayCabinetDialog(out string userText)
        {
            ManualResetEvent dialogCloseEvent = new(false);
            bool okPressed = false;
            string inputText = "My Amiibo";
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    _parent.ViewModel.AppHost.NpadManager.BlockInputUpdates();
                    SoftwareKeyboardUIArgs args = new SoftwareKeyboardUIArgs();
                    args.KeyboardMode = KeyboardMode.Default;
                    args.InitialText = "Ryujinx";
                    args.StringLengthMin = 1;
                    args.StringLengthMax = 25;
                    (UserResult result, string userInput) = await SwkbdAppletDialog.ShowInputDialog(LocaleManager.Instance[LocaleKeys.CabinetDialog], args);
                    if (result == UserResult.Ok)
                    {
                        inputText = userInput;
                        okPressed = true;
                    }
                }
                finally
                {
                    dialogCloseEvent.Set();
                }
            });
            dialogCloseEvent.WaitOne();
            _parent.ViewModel.AppHost.NpadManager.UnblockInputUpdates();
            userText = inputText;
            return okPressed;
        }

        public void DisplayCabinetMessageDialog()
        {
            ManualResetEvent dialogCloseEvent = new(false);
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                dialogCloseEvent.Set();
                await ContentDialogHelper.CreateInfoDialog(LocaleManager.Instance[LocaleKeys.CabinetScanDialog],
                string.Empty,
                LocaleManager.Instance[LocaleKeys.InputDialogOk],
                string.Empty,
                LocaleManager.Instance[LocaleKeys.CabinetTitle]);
            });
            dialogCloseEvent.WaitOne();
        }


        public void ExecuteProgram(Switch device, ProgramSpecifyKind kind, ulong value)
        {
            device.Configuration.UserChannelPersistence.ExecuteProgram(kind, value);
            _parent.ViewModel.AppHost?.Stop();
        }

        public bool DisplayErrorAppletDialog(string title, string message, string[] buttons)
        {
            ManualResetEvent dialogCloseEvent = new(false);

            bool showDetails = false;

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    ErrorAppletWindow msgDialog = new(_parent, buttons, message)
                    {
                        Title = title,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Width = 400
                    };

                    object response = await msgDialog.Run();

                    if (response != null && buttons is { Length: > 1 } && (int)response != buttons.Length - 1)
                    {
                        showDetails = true;
                    }

                    dialogCloseEvent.Set();

                    msgDialog.Close();
                }
                catch (Exception ex)
                {
                    dialogCloseEvent.Set();

                    await ContentDialogHelper.CreateErrorDialog(LocaleManager.Instance.UpdateAndGetDynamicValue(LocaleKeys.DialogErrorAppletErrorExceptionMessage, ex));
                }
            });

            dialogCloseEvent.WaitOne();

            return showDetails;
        }

        public IDynamicTextInputHandler CreateDynamicTextInputHandler() => new AvaloniaDynamicTextInputHandler(_parent);
    }
}
