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
    public partial class SettingsGuardianView : ContentView
    {
        private Xamarin.Forms.Page _nextPage;

        public SettingsGuardianView(Xamarin.Forms.Page nextPage = null)
        {
            if (nextPage!=null)
            {
                _nextPage = nextPage;
            }
            InitializeComponent();
            swFallDetection.IsToggled = Preferences.Get(Settings.FallDetecion, false);
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
                case "swFallDetection":
                    toggle(Settings.SOS_BUTTON_VISIBLE, swFallDetection);
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
            if (swFallDetection.IsToggled)
            {

                await vm.EnableFallDetection();
            }
            else
            {
                vm.DisableFallDetection();
            }
            if (_nextPage != null)
            {
                Device.BeginInvokeOnMainThread(() => Navigation.PushAsync(_nextPage,false));
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
            }
                           
        }
    }
}