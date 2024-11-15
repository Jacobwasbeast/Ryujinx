using Ryujinx.Common.Configuration;
using Ryujinx.Common.Memory;
using Ryujinx.Common.Utilities;
using Ryujinx.Cpu;
using Ryujinx.Graphics.Gpu;
using Ryujinx.HLE.HOS.Services.Mii;
using Ryujinx.HLE.HOS.Services.Mii.Types;
using Ryujinx.HLE.HOS.Services.Nfc.Nfp.NfpManager;
using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Threading;
using Gommon;
namespace Ryujinx.HLE.HOS.Services.Nfc.Nfp
{
    static class VirtualAmiibo
    {
        private static uint _openedApplicationAreaId;

        private static readonly AmiiboJsonSerializerContext _serializerContext = AmiiboJsonSerializerContext.Default;

        public static byte[] GenerateUuid(string amiiboId, bool useRandomUuid)
        {
            if (useRandomUuid)
            {
                return GenerateRandomUuid();
            }

            VirtualAmiiboFile virtualAmiiboFile = LoadAmiiboFile(amiiboId);

            if (virtualAmiiboFile.TagUuid.Length == 0)
            {
                virtualAmiiboFile.TagUuid = GenerateRandomUuid();

                SaveAmiiboFile(virtualAmiiboFile);
            }

            return virtualAmiiboFile.TagUuid;
        }

        private static byte[] GenerateRandomUuid()
        {
            byte[] uuid = new byte[9];

            Random.Shared.NextBytes(uuid);

            uuid[3] = (byte)(0x88 ^ uuid[0] ^ uuid[1] ^ uuid[2]);
            uuid[8] = (byte)(uuid[3] ^ uuid[4] ^ uuid[5] ^ uuid[6]);

            return uuid;
        }

        public static CommonInfo GetCommonInfo(string amiiboId)
        {
            VirtualAmiiboFile amiiboFile = LoadAmiiboFile(amiiboId);

            return new CommonInfo()
            {
                LastWriteYear = (ushort)amiiboFile.LastWriteDate.Year,
                LastWriteMonth = (byte)amiiboFile.LastWriteDate.Month,
                LastWriteDay = (byte)amiiboFile.LastWriteDate.Day,
                WriteCounter = amiiboFile.WriteCounter,
                Version = 1,
                ApplicationAreaSize = AmiiboConstants.ApplicationAreaSize,
                Reserved = new Array52<byte>(),
            };
        }

        public static RegisterInfo GetRegisterInfo(ITickSource tickSource, string amiiboId, string userName)
        {
            VirtualAmiiboFile amiiboFile = LoadAmiiboFile(amiiboId);
            string filePath = AppDataManager.AmiiboFileLocation;
            string nickname = "Ryujinx";
            if (!filePath.IsNullOrEmpty())
            {
                nickname = Path.GetFileNameWithoutExtension(filePath);
            }
           
            UtilityImpl utilityImpl = new(tickSource);
            CharInfo charInfo = new();

            charInfo.SetFromStoreData(StoreData.BuildDefault(utilityImpl, 0));

            // This is the player's name
            charInfo.Nickname = Nickname.FromString(userName);

            RegisterInfo registerInfo = new()
            {
                MiiCharInfo = charInfo,
                FirstWriteYear = (ushort)amiiboFile.FirstWriteDate.Year,
                FirstWriteMonth = (byte)amiiboFile.FirstWriteDate.Month,
                FirstWriteDay = (byte)amiiboFile.FirstWriteDate.Day,
                FontRegion = 0,
                Reserved1 = new Array64<byte>(),
                Reserved2 = new Array58<byte>(),
            };
            // This is the amiibo's name
            byte[] nicknameBytes = System.Text.Encoding.UTF8.GetBytes(nickname);
            nicknameBytes.CopyTo(registerInfo.Nickname.AsSpan());

            return registerInfo;
        }

        public static bool OpenApplicationArea(string amiiboId, uint applicationAreaId)
        {
            VirtualAmiiboFile virtualAmiiboFile = LoadAmiiboFile(amiiboId);

            if (virtualAmiiboFile.ApplicationAreas.Exists(item => item.ApplicationAreaId == applicationAreaId))
            {
                _openedApplicationAreaId = applicationAreaId;

                return true;
            }

            return false;
        }

        public static byte[] GetApplicationArea(string amiiboId)
        {
            VirtualAmiiboFile virtualAmiiboFile = LoadAmiiboFile(amiiboId);

            foreach (VirtualAmiiboApplicationArea applicationArea in virtualAmiiboFile.ApplicationAreas)
            {
                if (applicationArea.ApplicationAreaId == _openedApplicationAreaId)
                {
                    return applicationArea.ApplicationArea;
                }
            }

            return Array.Empty<byte>();
        }

        public static bool CreateApplicationArea(string amiiboId, uint applicationAreaId, byte[] applicationAreaData)
        {
            VirtualAmiiboFile virtualAmiiboFile = LoadAmiiboFile(amiiboId);

            if (virtualAmiiboFile.ApplicationAreas.Exists(item => item.ApplicationAreaId == applicationAreaId))
            {
                return false;
            }

            virtualAmiiboFile.ApplicationAreas.Add(new VirtualAmiiboApplicationArea()
            {
                ApplicationAreaId = applicationAreaId,
                ApplicationArea = applicationAreaData,
            });

            SaveAmiiboFile(virtualAmiiboFile);

            return true;
        }

        public static void SetApplicationArea(string amiiboId, byte[] applicationAreaData)
        {
            VirtualAmiiboFile virtualAmiiboFile = LoadAmiiboFile(amiiboId);

            if (virtualAmiiboFile.ApplicationAreas.Exists(item => item.ApplicationAreaId == _openedApplicationAreaId))
            {
                for (int i = 0; i < virtualAmiiboFile.ApplicationAreas.Count; i++)
                {
                    if (virtualAmiiboFile.ApplicationAreas[i].ApplicationAreaId == _openedApplicationAreaId)
                    {
                        virtualAmiiboFile.ApplicationAreas[i] = new VirtualAmiiboApplicationArea()
                        {
                            ApplicationAreaId = _openedApplicationAreaId,
                            ApplicationArea = applicationAreaData,
                        };

                        break;
                    }
                }

                SaveAmiiboFile(virtualAmiiboFile);
            }
        }

        private static VirtualAmiiboFile LoadAmiiboFile(string amiiboId)
        {
            // Set up directory and file paths
            var amiiboDirPath = Path.Join(AppDataManager.BaseDirPath, "system", "amiibo");
            Directory.CreateDirectory(amiiboDirPath);
            string defaultFilePath = Path.Combine(amiiboDirPath, $"{amiiboId}.json");
            if (!File.Exists(defaultFilePath))
            {
                VirtualAmiiboFile virtualAmiiboFileDef = new VirtualAmiiboFile
                {
                    FileVersion = 0,
                    TagUuid = Array.Empty<byte>(),
                    AmiiboId = amiiboId,
                    FirstWriteDate = DateTime.Now,
                    LastWriteDate = DateTime.Now,
                    WriteCounter = 0,
                    ApplicationAreas = new List<VirtualAmiiboApplicationArea>(),
                };

                SaveAmiiboFile(virtualAmiiboFileDef, defaultFilePath);
            }
            string filePath;
            if (AppDataManager.AmiiboFileLocation.IsNullOrEmpty())
            {
                var dialog = new OpenFileDialog
                {
                    Title = $"Load Amiibo File: {amiiboId}",
                    InitialFileName = amiiboId,
                    AllowMultiple = false,
                    Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "JSON", Extensions = new List<string> { "json" } }
                },
                    Directory = amiiboDirPath
                };

                filePath = null;

                // Show dialog and get the file path using UI thread
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (filePath == null)
                    {
                        var window = HiddenWindow();
                        var result = await dialog.ShowAsync(window);
                        filePath = result?.Length > 0 ? result[0] : defaultFilePath;
                        window.Close();
                    }
                }).Wait();
                AppDataManager.AmiiboFileLocation = filePath;
            }
            filePath = AppDataManager.AmiiboFileLocation;
            VirtualAmiiboFile virtualAmiiboFile;

            if (File.Exists(filePath))
            {
                // Deserialize the file
                virtualAmiiboFile = JsonHelper.DeserializeFromFile(filePath, _serializerContext.VirtualAmiiboFile);
            }
            else
            {
                // Create new file if it doesn't exist
                virtualAmiiboFile = new VirtualAmiiboFile
                {
                    FileVersion = 0,
                    TagUuid = Array.Empty<byte>(),
                    AmiiboId = amiiboId,
                    FirstWriteDate = DateTime.Now,
                    LastWriteDate = DateTime.Now,
                    WriteCounter = 0,
                    ApplicationAreas = new List<VirtualAmiiboApplicationArea>(),
                };

                SaveAmiiboFile(virtualAmiiboFile);
            }

            return virtualAmiiboFile;
        }

        private static void SaveAmiiboFile(VirtualAmiiboFile virtualAmiiboFile)
        {
            string defaultFilePath = Path.Join(AppDataManager.BaseDirPath, "system", "amiibo", $"{virtualAmiiboFile.AmiiboId}.json");
            var dialog = new SaveFileDialog()
            {
                Title = "Save Amiibo File",
                InitialFileName = virtualAmiiboFile.AmiiboId,
                DefaultExtension = "json",
                Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter()
                    {
                        Name = "JSON",
                        Extensions = new List<string>() { "json" }
                    }
                },
                Directory = Path.GetDirectoryName(defaultFilePath),
            };
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                var window = HiddenWindow();
                var pathTask = await dialog.ShowAsync(window);

                if (!string.IsNullOrWhiteSpace(pathTask))
                {
                    JsonHelper.SerializeToFile(pathTask, virtualAmiiboFile, _serializerContext.VirtualAmiiboFile);
                }

                window.Close(); 
            }).Wait();
            AppDataManager.AmiiboFileLocation = null;
        }
        private static void SaveAmiiboFile(VirtualAmiiboFile virtualAmiiboFile, string path)
        {
            JsonHelper.SerializeToFile(path, virtualAmiiboFile, _serializerContext.VirtualAmiiboFile);
        }

        private static Avalonia.Controls.Window HiddenWindow()
        {
            var hiddenWindow = new Avalonia.Controls.Window
            {
                Width = 100,
                Height = 100,
                Opacity = 0,
                ShowInTaskbar = false
            };
            return hiddenWindow;
        }
        public class App : Application { }
    }
}
