using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Services.Alert;
using AlertApp.Services.Settings;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AlertApp.ViewModels
{
    public class SendingAlertPageViewModel : BaseViewModel, ISendAlert
    {
        #region Services
        readonly IAlertService _alertService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties

        private string _AlertTypeLabel;
        public string AlertTypeLabel
        {
            get { return _AlertTypeLabel; }
            set
            {
                _AlertTypeLabel = value;
                OnPropertyChanged("AlertTypeLabel");
            }
        }

        public AlertType alertType { get; set; }

        #endregion

        public SendingAlertPageViewModel(IAlertService alertService, ILocalSettingsService localSettingsService, AlertType alertType)
        {
            _alertService = alertService;
            _localSettingsService = localSettingsService;
            this.alertType = alertType;
            switch (alertType)
            {
                case AlertType.UserAlert:
                    AlertTypeLabel = "SOS PRESSED!";
                    break;
            }
        }

        public async void SendAlert()
        {
            SetBusy(true);
            var location = await GetCurrentLocation();
            var sendOK = await _alertService.SendAlert(await _localSettingsService.GetAuthToken(), (location != null && location.Ok) ? location.Location.Latitude : (double?)null, (location != null && location.Ok) ? location.Location.Longitude : (double?)null, (int)this.alertType);
            if (sendOK != null)
            {
                await NavigationService.PopAsync();
            }
            SetBusy(false);
        }
        private async Task<LocationResult> GetCurrentLocation()
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
                    return new LocationResult { Ok = false, ErroMessage = "Permissions Denied. Unable to get location." };
                }
                
                var location = await Geolocation.GetLastKnownLocationAsync();                
                if (location != null)
                {
                    
                    return new LocationResult { Ok = true, Location = location }; ;
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
            //SetBusy(false);
            return new LocationResult { Ok = false };
        }
        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion

        #region ISendAlert

        public void SendUserAlert()
        {
            SendAlert();
        }

        public Task<string> GetApplicationPin()
        {
            return _localSettingsService.GetApplicationPin();
        }
        #endregion
    }
}
