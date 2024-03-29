﻿using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.ViewModels;
using AlertApp.Views;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static PCLCrypto.WinRTCrypto;
using System.Linq;
using AlertApp.Utils;
using AlertApp.Services.Profile;

namespace AlertApp.ViewModels
{
    public class EnterApplicationPinCodePageViewModel : BaseViewModel
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        readonly ICryptographyService _cryptohraphyService;
        readonly IUserProfileService _userProfileService;
        #endregion

        #region Properties


        private string _Pin1;

        public string Pin1
        {
            get { return _Pin1; }
            set
            {
                _Pin1 = value;
                OnPropertyChanged("CanContinue");
                OnPropertyChanged("CodeCompleted");
                OnPropertyChanged("PromtText");
            }
        }
        private string _Pin2;

        public string Pin2
        {
            get { return _Pin2; }
            set
            {
                _Pin2 = value;
                OnPropertyChanged("CanContinue");
                OnPropertyChanged("CodeCompleted");
                OnPropertyChanged("PromtText");
            }
        }
        private string _Pin4;

        public string Pin3
        {
            get { return _Pin3; }
            set
            {
                _Pin3 = value;
                OnPropertyChanged("CanContinue");
                OnPropertyChanged("CodeCompleted");
                OnPropertyChanged("PromtText");
            }
        }

        private string _Pin3;

        public string Pin4
        {
            get { return _Pin4; }
            set
            {
                _Pin4 = value;
                OnPropertyChanged("CanContinue");
                OnPropertyChanged("CodeCompleted");
                OnPropertyChanged("PromtText");
            }
        }


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


        public bool CanContinue => !string.IsNullOrWhiteSpace(Pin1) && !string.IsNullOrWhiteSpace(Pin2) && !string.IsNullOrWhiteSpace(Pin3) && !string.IsNullOrWhiteSpace(Pin4);

        public bool CodeCompleted => !CanContinue;

        public string PromtText => CodeCompleted ? AppResources.RegistrationCreatePinPromt : AppResources.RegistrationEnterPinPromt;



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

        public EnterApplicationPinCodePageViewModel(ILocalSettingsService localSettingsService, ICryptographyService cryptohraphyService, IUserProfileService userProfileService)
        {
            _localSettingsService = localSettingsService;
            _cryptohraphyService = cryptohraphyService;
            LocationTracking = true;
            _userProfileService = userProfileService;
        }

        public async void Continue()
        {
            if (string.IsNullOrWhiteSpace(Pin1) || string.IsNullOrWhiteSpace(Pin2) || string.IsNullOrWhiteSpace(Pin3) || string.IsNullOrWhiteSpace(Pin4))
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
                await Task.Run(() => _cryptohraphyService.GenerateKeys(String.Format("{0}{1}{2}{3}", Pin1, Pin2, Pin3, Pin4)));

                //var storedProfile = await _userProfileService.StoreProfile(new Dictionary<string, string>(), await _localSettingsService.GetAuthToken(), await _localSettingsService.GetPublicKey());
                //if (storedProfile.IsOk)
                //{
                //    Preferences.Set(Settings.HasFinishRegistration, true);
                //    Device.BeginInvokeOnMainThread(() =>
                //    {
                //        Application.Current.MainPage.Navigation.InsertPageBefore(new MainPage(), Application.Current.MainPage.Navigation.NavigationStack.First());
                //        Application.Current.MainPage.Navigation.PopToRootAsync();
                //    });

                //}
                //else if (!storedProfile.IsOnline)
                //{
                //    showOKMessage(AppResources.Error, "Please check your internet connection.");
                //}

                SetBusy(false);
                //await Application.Current.MainPage.Navigation.PushAsync(new SettingContainerPage(AppResources.SettingPermissionTitle, AppResources.FallDetectionSetting, new SettingsGuardianView(new RegistrationFieldsPage(App.TempRegistrationFields))), false);
                await Application.Current.MainPage.Navigation.PushAsync(new RegistrationFieldsPage(null), false);
                //we keep TempRegistrationFields in static field in App.xaml.cs.
                //await Application.Current.MainPage.Navigation.PushAsync(new RegistrationFieldsPage(App.TempRegistrationFields), false);
                //Preferences.Set(Settings.HasFinishRegistration, true);
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    Application.Current.MainPage.Navigation.InsertPageBefore(new MainPage(), Application.Current.MainPage.Navigation.NavigationStack.First());
                //    Application.Current.MainPage.Navigation.PopToRootAsync();
                //});


            }
        }

        private async Task<LocationResult> GetLocationStatus()
        {
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    locationPermissionStatus = results;
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
