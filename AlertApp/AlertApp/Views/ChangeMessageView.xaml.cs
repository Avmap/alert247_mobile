using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Utils;
using AlertApp.Services.Settings;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeMessageView : ContentView
    {
        readonly ILocalSettingsService _localSettingsService;
        public ChangeMessageView()
        {
            InitializeComponent();
            _localSettingsService = new LocalSettingsService();

            input.Text= Preferences.Get(Settings.SOS_MESSAGE, "SOS");
        }
        
        private void ConfirmSettingsClick(object sender, EventArgs e)
        {
            
            // var confirmView = new ConfirmChangeView();
            // var page = new SettingContainerPage(AppResources.SettingAccountTitle, AppResources.Confirmation, confirmView);
            // page.Disappearing += (sender2, e2) =>
            // {
            //     if (confirmView.Confirmed)
            //     {
            //         SaveChanges();
            //     }
            // };
            
            SaveChanges();

            //Navigation.PushModalAsync(page);
            

        }
        private void SaveChanges()
        {
            Preferences.Set(Settings.SOS_MESSAGE, input.Text);
            Device.BeginInvokeOnMainThread(() => Navigation.PopAsync(false));
        }
    }
}