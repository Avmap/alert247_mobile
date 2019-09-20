using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using PCLCrypto;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PCLCrypto.WinRTCrypto;

namespace AlertApp.ViewModels
{
    public class EnterApplicationPinCodePageViewModel : BaseViewModel
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        readonly ICryptographyService _cryptohraphyService;
        #endregion

        #region Properties
        public string Pin { get; set; }
        private bool _LocationTracking;

        public bool LocationTracking
        {
            get { return _LocationTracking; }
            set
            {
                _LocationTracking = value;
                OnPropertyChanged("LocationTracking");
            }
        }

        #endregion

        #region Commands
        private ICommand _ContinueCommand;
        public ICommand ContinueCommand
        {
            get
            {
                return _ContinueCommand ?? (_ContinueCommand = new Command(Continue, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        public EnterApplicationPinCodePageViewModel(ILocalSettingsService localSettingsService, ICryptographyService cryptohraphyService)
        {
            _localSettingsService = localSettingsService;
            _cryptohraphyService = cryptohraphyService;
            LocationTracking = true;
        }

        public async void Continue()
        {
            if (string.IsNullOrWhiteSpace(Pin))
            {
                showOKMessage(AppResources.Warning, AppResources.WarningFillPin);
            }
            else
            {
                if (LocationTracking)
                {
                    var locationStatus = await GetLocationStatus();
                    if (!locationStatus.Ok)
                    {
                        showOKMessage(AppResources.Warning, locationStatus.ErroMessage);
                        return;
                    }
                }
                _localSettingsService.SaveSendLocationSetting(LocationTracking);
                SetBusy(true);
                await Task.Run(() => _cryptohraphyService.GenerateKeys(Pin));
                SetBusy(false);
                //we keep TempRegistrationFields in static field in App.xaml.cs.
                await NavigationService.PushAsync(new RegistrationFieldsPage(App.TempRegistrationFields), true);
            }
        }

        private async Task<LocationResult> GetLocationStatus()
        {
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
                    return new LocationResult { Ok = false, ErroMessage = AppResources.UnableToGetLocationPermission };
                }

                return new LocationResult { Ok = true }; ;
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
            return new LocationResult { Ok = false };
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)ContinueCommand).ChangeCanExecute();
            });
        }
        #endregion

    }
}
