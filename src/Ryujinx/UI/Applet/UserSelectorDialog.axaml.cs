using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using Ryujinx.Ava.Common.Locale;
using Ryujinx.Ava.UI.Controls;
using Ryujinx.Ava.UI.Helpers;
using Ryujinx.Ava.UI.ViewModels;
using Ryujinx.Common.Logging;
using Ryujinx.HLE.HOS.Applets;
using Ryujinx.HLE.HOS.Services.Account.Acc;
using shaderc;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Button = Avalonia.Controls.Button;
using UserProfile = Ryujinx.Ava.UI.Models.UserProfile;
using UserProfileSft = Ryujinx.HLE.HOS.Services.Account.Acc.UserProfile;

namespace Ryujinx.Ava.UI.Applet
{
    public partial class UserSelectorDialog : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private UserId _selectedUserId;

        public ObservableCollection<BaseModel> Profiles { get; set; }

        public UserSelectorDialog(ObservableCollection<BaseModel> Profiles)
        {
            InitializeComponent();
            this.Profiles = Profiles;
            DataContext = this;  
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Grid_PointerEntered(object sender, PointerEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is UserProfile profile)
            {
                profile.IsPointerOver = true;
            }
        }

        private void Grid_OnPointerExited(object sender, PointerEventArgs e)
        {
            if (sender is Grid grid && grid.DataContext is UserProfile profile)
            {
                profile.IsPointerOver = false;
            }
        }

        private void ProfilesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox)
            {
                int selectedIndex = listBox.SelectedIndex;

                if (selectedIndex >= 0 && selectedIndex < Profiles.Count)
                {
                    if (Profiles[selectedIndex] is UserProfile userProfile)
                    {
                        _selectedUserId = userProfile.UserId;
                        Logger.Info?.Print(LogClass.UI, $"Selected user: {userProfile.UserId}");
                        var NewProfiles = new ObservableCollection<BaseModel>();

                        foreach (var item in Profiles)
                        {
                            UserProfile originalItem = (UserProfile)item;
                            UserProfileSft profile = new (originalItem.UserId, originalItem.Name,
                                originalItem.Image);
                            if (profile.UserId == _selectedUserId)
                            {
                                profile.AccountState = AccountState.Open;
                            }
                            NewProfiles.Add(new UserProfile(profile, new NavigationDialogHost()));
                        }

                        Profiles = NewProfiles;
                        OnPropertyChanged(nameof(Profiles));
                    }
                }
            }
        }

        public static async Task<(UserId id, bool result)> ShowInputDialog(UserSelectorDialog content)
        {
            ContentDialog contentDialog = new()
            {
                Title = LocaleManager.Instance[LocaleKeys.UserProfileWindowTitle],
                PrimaryButtonText = LocaleManager.Instance[LocaleKeys.Continue],
                SecondaryButtonText = string.Empty,
                CloseButtonText = string.Empty,
                Content = content,
                Padding = new Thickness(0)
            };

            UserId result = UserId.Null;
            bool input = false;

            void Handler(ContentDialog sender, ContentDialogClosedEventArgs eventArgs)
            {
                if (eventArgs.Result == ContentDialogResult.Primary)
                {
                    UserSelectorDialog view = (UserSelectorDialog)contentDialog.Content;
                    result = view?._selectedUserId ?? UserId.Null;
                    input = true;
                }
            }

            contentDialog.Closed += Handler;

            await ContentDialogHelper.ShowAsync(contentDialog);

            return (result, input);
        }
    }
}
