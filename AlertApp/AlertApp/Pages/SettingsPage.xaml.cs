using AlertApp.Resx;
using AlertApp.Utils;
using AlertApp.ViewModels;
using AlertApp.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        bool codeTrigger = false;
        public SettingsPage()
        {
            InitializeComponent();
            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
        }



        private void Language_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingLanguageTitle, AppResources.SettingLanguageSubTitle, new SelectLanguageView()), false);
        }
        private void ButtonsView_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingButtonsTitle, AppResources.SettingButtonsSubTitle, new SettingsButtonsView()), false);
        }

        private void Location_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.SettingPermissionLocationSubTitle, new SettingsLocationView()), false);
        }
        private void Guardian_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.AllwaysOn, new SettingsAlwaysOnView()), false);
        }

        private void Fall_Detection_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.FallDetectionSetting, new SettingsGuardianView()), false);
        }

        private void Contacts_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.ContactPermissionSettingTitle, new SettingsContactsView()), false);
        }
        private void ChangePin_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.SettingChangePin, new SettingsChangePinView()), false);
        }

        private void History_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.SettingHistory, new SettingsAccountHistoryView()), false);
        }

        private void Delete_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.SettingDeleteAccount, new SettingsChangePinView()), false);
        }
        private void Fall_Detector_Settings_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.FallDetectorSettings, new SettingsFallDetectorView()), false);
        }
        private void UpdateProfile_Tap(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.SettingUpdateProfle, new UpdateProfileView()), false);
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((SettingsPageViewModel)this.BindingContext).HideShowButtons();
        }
    }
}