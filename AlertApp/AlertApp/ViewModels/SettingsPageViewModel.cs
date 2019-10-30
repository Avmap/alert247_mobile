using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Settings;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Services
        readonly IGuardian _guardian;
        readonly ILocalSettingsService _localSettingsService;
        #endregion
        
        #region Properties

        public bool AllwaysOn { get; set; }
        #endregion

        #region Commands

        private ICommand _OpenHomeScreen;
        public ICommand OpenHomeScreen
        {
            get
            {
                return _OpenHomeScreen ?? (_OpenHomeScreen = new Command(Back, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _OpenContactsScreen;
        public ICommand OpenContactsScreen
        {
            get
            {
                return _OpenContactsScreen ?? (_OpenContactsScreen = new Command(NavigateToContactScreen, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        public SettingsPageViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
            _guardian = DependencyService.Get<IGuardian>();
            AllwaysOn = _localSettingsService.GetAlwaysOn();
        }

        public async Task<LocationResult> EnableGuardian()
        {
            var result = new LocationResult { Ok = true };
            try
            {           
                var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location });
                    locationPermissionStatus = results[Permission.Location];
                }

                if (locationPermissionStatus != PermissionStatus.Granted)
                {                    
                    result = new LocationResult { Ok = false, ErroMessage = "Location permissions Denied. Unable to start guardian." };
                }


                var locationSettingsService = DependencyService.Get<ILocationSettings>();
                if (!locationSettingsService.IsLocationEnabled())
                {
                    result = new LocationResult { Ok = false, ErroMessage = "To open guardian turn on device location." };
                }


                if (result.Ok)
                {
                    AllwaysOn = true;
                    _guardian.StartGuardianService();
                    _localSettingsService.SetAlwaysOn(true);
                }
                else
                {
                    showOKMessage(AppResources.Warning, result.ErroMessage);
                }
                

                return result;
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
            //SetBusy(false);   
            return result;
        }

        public void DisableGuardian()
        {
            _guardian.StopGuardianService();
            _localSettingsService.SetAlwaysOn(false);
            AllwaysOn = false;
        }


        private async void Back()
        {
            await NavigationService.PopAsync(false);
        }

        public void HideShowButtons()
        {
            OnPropertyChanged("ShowContactsMenuButton");
            OnPropertyChanged("ShowInfoMenuButton");
        }

        private async void NavigateToContactScreen()
        {

            SetBusy(true);
            await NavigationService.PushAsync(new ManageContactsPage(), false);
            SetBusy(false);
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion
    }

}