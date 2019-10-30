using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Utils;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsLocationView : ContentView
    {
        public SettingsLocationView()
        {
            InitializeComponent();
            LOCATION.IsToggled = Preferences.Get(Settings.AlwaysOn, true);
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            toggleSetting(args.Parameter as string, true);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchView = sender as Switch;
            toggleSetting(switchView.Id.ToString(), true);
        }

        private void toggleSetting(string setting, bool fromCode)
        {
            switch (setting)
            {
                case "LOCATION":
                    toggle(Settings.SOS_BUTTON_VISIBLE, LOCATION);
                    break;

            }
        }
        private void toggle(string setting, Switch switchView)
        {
            var newValue = !switchView.IsToggled;
            switchView.IsToggled = !switchView.IsToggled;
        }

        private void ConfirmSettingsClick(object sender, EventArgs e)
        {
            var confirmView = new ConfirmChangeView();
            var page = new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.Confirmation, confirmView);
            page.Disappearing += (sender2, e2) =>
            {
                if (confirmView.Confirmed)
                {
                    SaveChanges();
                }
            };

            Navigation.PushModalAsync(page);
        }

        private async void SaveChanges()
        {
            var vm = this.BindingContext as SettingsPageViewModel;
            if (LOCATION.IsToggled)
            {

                await vm.EnableGuardian();
            }
            else
            {
                vm.DisableGuardian();
            }

        }
    }
}