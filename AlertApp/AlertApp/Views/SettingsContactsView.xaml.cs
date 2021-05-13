using AlertApp.Pages;
using AlertApp.Resx;
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
    public partial class SettingsContactsView : ContentView
    {

        public SettingsContactsView()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                var contactPermissionStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                if (contactPermissionStatus == PermissionStatus.Granted)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        activeLayout.IsVisible = true;
                        inactiveLayout.IsVisible = false;
                    });

                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        activeLayout.IsVisible = false;
                        inactiveLayout.IsVisible = true;
                    });
                }
            });


            this.BindingContext = ViewModelLocator.Instance.Resolve<SettingsPageViewModel>();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var args = e as Xamarin.Forms.TappedEventArgs;
            Task.Run(async () =>
            {
                var results = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                var contactPermissionStatus = results;
                if (contactPermissionStatus == PermissionStatus.Granted)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        activeLayout.IsVisible = true;
                        inactiveLayout.IsVisible = false;
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        activeLayout.IsVisible = false;
                        inactiveLayout.IsVisible = true;
                    });
                }
            });
        }

    }
}