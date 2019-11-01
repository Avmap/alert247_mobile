using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsContactsView : ContentView
    {
        public bool fromCode { get; set; }
        public SettingsContactsView()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                if (contactPermissionStatus == PermissionStatus.Granted)
                {
                    CONTACTS.IsToggled = true;
                }
                else
                {
                    CONTACTS.IsToggled = false;
                }
            });


            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            CONTACTS.IsToggled = !CONTACTS.IsToggled;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchView = sender as Switch;
            if (!fromCode)
            {
                toggleSetting(switchView.Id.ToString(), true);
            }
        
        }

        private void toggleSetting(string setting, bool fromCode)
        {
            Task.Run(async () =>
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Contacts });
                var contactPermissionStatus = results[Permission.Contacts];
                fromCode = true;
                if (contactPermissionStatus == PermissionStatus.Granted)
                {
                    CONTACTS.IsToggled = true;
                }
                else
                {
                    CONTACTS.IsToggled = false;
                }
                fromCode = true;
            });
        }
        private void toggle(string setting, Switch switchView)
        {
            var newValue = !switchView.IsToggled;
            switchView.IsToggled = !switchView.IsToggled;
        }

        //private void ConfirmSettingsClick(object sender, EventArgs e)
        //{
        //    var confirmView = new ConfirmChangeView();
        //    var page = new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.Confirmation, confirmView);
        //    page.Disappearing += (sender2, e2) =>
        //    {
        //        if (confirmView.Confirmed)
        //        {
        //            SaveChanges();
        //        }
        //    };

        //    Navigation.PushModalAsync(page);
        //}

        //private async void SaveChanges()
        //{
        //    var vm = this.BindingContext as SettingsPageViewModel;
        //    if (CONTACTS.IsToggled)
        //    {


        //    }
        //    else
        //    {
        //     //   vm.DisableGuardian();
        //    }

        //}
    }
}