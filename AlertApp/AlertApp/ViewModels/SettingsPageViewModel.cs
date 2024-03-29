﻿using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        readonly ICryptographyService _cryptographyService;
        #endregion

        #region Properties

        public string Version => String.Format("{0} {1}", "Version", VersionTracking.CurrentVersion);
        #endregion

        #region Commands

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

        private ICommand _LinkCommand;
        public ICommand LinkCommand
        {
            get
            {
                return _LinkCommand ?? (_LinkCommand = new Command<string>(OpenLink, (url) =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _LinkPoweredByCommand;
        public ICommand LinkPoweredByCommand
        {
            get
            {
                return _LinkPoweredByCommand ?? (_LinkPoweredByCommand = new Command<string>(OpenPoweredByLink, (url) =>
                {
                    return !Busy;
                }));
            }
        }


        #endregion

        public SettingsPageViewModel(ILocalSettingsService localSettingsService, ICryptographyService cryptographyService)
        {
            _localSettingsService = localSettingsService;
            _guardian = DependencyService.Get<IGuardian>();
            _cryptographyService = cryptographyService;
        }

        public async Task<LocationResult> EnableFallDetection()
        {
            var result = new LocationResult { Ok = true };
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.LocationAlways>();
                    locationPermissionStatus = results;
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

                if (_guardian != null)
                {
                    if (result.Ok)
                    {
                        _localSettingsService.SetFallDetection(true);
                        MessagingCenter.Send((BaseViewModel)this, StartStopFallDetectionEvent.Event, new StartStopFallDetectionEvent { Start = true });
                    }
                    else
                    {
                        showOKMessage(AppResources.Warning, result.ErroMessage);
                    }
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
        public void DisableFallDetection()
        {
            MessagingCenter.Send((BaseViewModel)this, StartStopFallDetectionEvent.Event, new StartStopFallDetectionEvent { Stop = true });
            _localSettingsService.SetFallDetection(false);
        }

        public async Task<LocationResult> EnableAllwaysOn()
        {
            var result = new LocationResult { Ok = true };
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (locationPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await Permissions.RequestAsync<Permissions.LocationAlways>();
                    locationPermissionStatus = results;
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

                if (_guardian != null)
                {
                    if (result.Ok)
                    {
                        _guardian.StartGuardianService();
                        _localSettingsService.SetAlwaysOn(true);
                    }
                    else
                    {
                        showOKMessage(AppResources.Warning, result.ErroMessage);
                    }
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
        public void DisableAllwaysOn()
        {
            DisableFallDetection();
            _guardian.StopGuardianService();
            _localSettingsService.SetAlwaysOn(false);
        }

        public async Task<LocationResult> RequestLocation()
        {
            var result = new LocationResult { Ok = true };
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
                    result = new LocationResult { Ok = false, ErroMessage = "Location permissions Denied. Unable to start guardian." };
                }


                var locationSettingsService = DependencyService.Get<ILocationSettings>();
                if (!locationSettingsService.IsLocationEnabled())
                {
                    result = new LocationResult { Ok = false, ErroMessage = "To open guardian turn on device location." };
                }


                if (result.Ok)
                {
                    //  _guardian.StartGuardianService();
                    _localSettingsService.SaveSendLocationSetting(true);
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


        public void DisableSendLocation()
        {
            _localSettingsService.SaveSendLocationSetting(false);
        }
        private async void Back()
        {
            await Application.Current.MainPage.Navigation.PopAsync(false);
        }

        public void HideShowButtons()
        {
            //OnPropertyChanged("ShowContactsMenuButton");
            //OnPropertyChanged("ShowInfoMenuButton");
        }

        private async void NavigateToContactScreen()
        {

            SetBusy(true);
            await Application.Current.MainPage.Navigation.PushAsync(new ManageContactsPage(), false);
            SetBusy(false);
        }

        private void OpenLink(string link)
        {
            var selectedLanguage = Preferences.Get(Utils.Settings.SelectedLanguage, "");
            selectedLanguage = selectedLanguage.Substring(0, 2);

            Launcher.OpenAsync(new Uri($"{link}/{selectedLanguage}"));
        }

        private void OpenPoweredByLink(string link)
        {
            Launcher.OpenAsync(new Uri(link));
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        public async Task OpenUserProfile()
        {
            var selectedLanguage = Preferences.Get(Utils.Settings.SelectedLanguage, "");
            selectedLanguage = selectedLanguage.Substring(0, 2);
            var mobilePhone = await _localSettingsService.GetMobilePhone();
            var user = $"{mobilePhone.Replace("+", string.Empty)}@alert247.gr";
            var urlSource = CodeSettings.UserProfilePage.Replace("$MOBILE$", HttpUtility.UrlEncode(user));
            urlSource = urlSource.Replace("$PIN$", await _localSettingsService.GetApplicationPin());
            urlSource = urlSource.Replace("$LANG$", selectedLanguage);
            await Browser.OpenAsync(new Uri(urlSource), BrowserLaunchMode.SystemPreferred);
        }
        #endregion
    }

}