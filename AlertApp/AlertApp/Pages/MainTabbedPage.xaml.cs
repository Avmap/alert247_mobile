using AlertApp.Infrastructure;
using AlertApp.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlertApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage() : this(null)
        {

        }

        public MainTabbedPage(NotificationAction notificationAction)
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = ViewModelLocator.Instance.Resolve<MainTabbedPageViewModel>();
            ToolbarItems.Add(new ToolbarItem
            {
                Icon = "manage_contact_primary.png",
                Command = ((MainTabbedPageViewModel)this.BindingContext).OpenContactsScreen,
            });

            if (notificationAction != null)
            {
                switch (notificationAction.Type)
                {
                    case (int)NotificationAction.ActionType.Sos:
                    //    navigate(notificationAction);
                        break;
                }
            }

            RequestPermissions();
        }

        private async void RequestPermissions()
        {
            try
            {
                var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                if (locationPermissionStatus != PermissionStatus.Granted || contactPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location, Permission.Contacts });
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }                        
        }

        //protected override bool OnBackButtonPressed()
        //{
        //    return true;
        //}
    }
}