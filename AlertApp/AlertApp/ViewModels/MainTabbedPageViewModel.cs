using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using Plugin.FirebasePushNotification;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class MainTabbedPageViewModel : BaseViewModel
    {

        #region Services
        readonly IUserProfileService _userProfileService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Commands

        private ICommand _SosCommand;
        public ICommand SosCommand
        {
            get
            {
                return _SosCommand ?? (_SosCommand = new Command(OpenSendAlertScreen, () =>
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

        private async void NavigateToContactScreen()
        {          
            await NavigationService.PushAsync(new ManageContactsPage(), true);
        }

        #endregion

        public MainTabbedPageViewModel(IUserProfileService userProfileService, ILocalSettingsService localSettingsService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
            PingServer();

        }

        private async void PingServer()
        {
            var userToken = await _localSettingsService.GetAuthToken();
            var firebaseToken = CrossFirebasePushNotification.Current.Token;
            Debug.WriteLine("Firebase", "New Token: " + firebaseToken);
            Location location = null;
            try
            {
                var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (locationPermissionStatus == PermissionStatus.Granted)
                {
                    location = await Geolocation.GetLastKnownLocationAsync();
                }
            }
            catch (Exception ex)
            {

            }
            await _userProfileService.Ping(userToken, location != null ? location.Latitude : (double?)null, location != null ? location.Longitude : (double?)null, firebaseToken);

            //var locales = await TextToSpeech.GetLocalesAsync();

            //// Grab the first locale
            //var locale = locales.LastOrDefault();
            //var settings = new SpeechOptions()
            //{
            //    Volume = 1f,
            //    Pitch = 1.0f,
            //    Locale = locale
            //};
            //await TextToSpeech.SpeakAsync("Hello from Barcelona", settings);

        }

        private async void OpenSendAlertScreen()
        {                        
            await NavigationService.PushAsync(new SendingAlertPage(Model.AlertType.UserAlert), true);
        }
        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion        
    }
}
