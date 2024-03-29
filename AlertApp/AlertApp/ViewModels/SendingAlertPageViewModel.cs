﻿using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.Services.Alert;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

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

        private bool _IsManualSos;

        public bool IsManualSos
        {
            get { return _IsManualSos; }
            set
            {
                _IsManualSos = value;
                OnPropertyChanged("IsManualSos");
            }
        }

        private string _SosTypeText;

        public string SosTypeText
        {
            get { return _SosTypeText; }
            set
            {
                _SosTypeText = value;
                OnPropertyChanged("SosTypeText");
            }
        }


        #endregion

        public SendingAlertPageViewModel(IAlertService alertService, ILocalSettingsService localSettingsService, AlertType alertType)
        {
            _alertService = alertService;
            _localSettingsService = localSettingsService;
            this.alertType = alertType;
            switch (alertType)
            {
                case AlertType.UserAlert:
                    IsManualSos = true;
                    SosTypeText = "SOS";
                    AlertTypeLabel = "SOS PRESSED!";
                    break;
                case AlertType.Fall:
                    SosTypeText = AppResources.FallDetectedSosText;
                    IsManualSos = false;
                    break;
            }
        }

        public async void SendAlert()
        {
            SetBusy(true);
            var location = await GetCurrentLocation();
            var alertResponse = await _alertService.SendAlert(await _localSettingsService.GetAuthToken(), (location != null && location.Ok) ? location.Location.Latitude : (double?)null, (location != null && location.Ok) ? location.Location.Longitude : (double?)null, (int)this.alertType);
            if (alertResponse != null && alertResponse.IsOk)
            {
                string message = "";
                var successSendContacts = 0;
                if (!(alertResponse.Result.Recipients is null))
                {
                    successSendContacts = alertResponse.Result.Recipients.Where(x => x.Value.StartsWith("NT")).Count();
                }
                if (successSendContacts>0)
                {
                    message += String.Format("{0} Alert {1} {2} {3}", AppResources.TheAlert, AppResources.SuccessSendAlertMessage, successSendContacts, AppResources.SuccessSendAlertMessageContacts);
                }
                if (alertResponse.Result.src == 1)
                {
                    message += String.Format("{0} Alert {1} {2}", AppResources.TheAlert, AppResources.SuccessSendAlertMessage, AppResources.SuccessSendAlertMessageSrc);
                }
                showOKMessage(AppResources.SuccessSendAlert, message);
                try
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                }
                catch
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync(false);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(alertResponse.ErrorCode) && alertResponse.ErrorCode == "ALERT_MESSAGE_MISSINGRECIPIENTS")
                {
                    showOKMessage(AppResources.Error, AppResources.NoAlertRecipients);
                }
                else if (!alertResponse.IsOnline)
                {
                    showOKMessage(AppResources.Error, AppResources.NoInternetConnection);
                }

                try
                {
                    await Application.Current.MainPage.Navigation.PopAsync(false);
                }
                catch
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync(false);
                }
            }
            SetBusy(false);
        }
        private async Task<LocationResult> GetCurrentLocation()
        {
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
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
