using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using Plugin.FirebasePushNotification;
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
    public class MainPageViewModel : BaseViewModel
    {
        #region Services
        readonly IUserProfileService _userProfileService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Commands

        private ICommand _OpenSettingsCommand;
        public ICommand OpenSettingsCommand
        {
            get
            {
                return _OpenSettingsCommand ?? (_OpenSettingsCommand = new Command(OpenSettingsScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _SosCommand;
        public ICommand SosCommand
        {
            get
            {
                return _SosCommand ?? (_SosCommand = new Command<string>(OpenSendAlertScreen, (alertType) =>
                {
                    return !Busy;
                }));
            }
        }


        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new Command(CancelSendAlert, () =>
                {
                    return true;
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

        #region Properties

        private string _SosButtonText;
        public string SosButtonText
        {
            get { return _SosButtonText; }
            set
            {
                _SosButtonText = value;
                OnPropertyChanged("SosButtonText");
            }
        }


        private string _CancelButtonText;
        public string CancelButtonText
        {
            get { return _CancelButtonText; }
            set
            {
                _CancelButtonText = value;
                OnPropertyChanged("CancelButtonText");
            }
        }


        public bool ShowSosButton
        {
            get { return !ShowCancelButton; }
        }

        private bool _ShowCancelButton;
        public bool ShowCancelButton
        {
            get { return _ShowCancelButton; }
            set
            {
                _ShowCancelButton = value;
                OnPropertyChanged("ShowCancelButton");
                OnPropertyChanged("ShowSosButton");
                OnPropertyChanged("ColorPressToCancel");
            }
        }

        bool stop = false;
        int maxSeconds = 5;
        int currentSecond = 0;
        Model.AlertType alertType;

        public Color ColorPressToCancel => ShowCancelButton ? Color.Black : Color.Transparent;
        #endregion

        public MainPageViewModel(IUserProfileService userProfileService, ILocalSettingsService localSettingsService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
            SosButtonText = "SOS";

            var appHasRun = _localSettingsService.GetAppHasRunSetting();
            if (!appHasRun)
            {
                var guardian = DependencyService.Get<IGuardian>();
                if (guardian != null)
                {
                    guardian.StartGuardianService();
                }
                _localSettingsService.SaveAppHasRunSetting(true);
            }
            PingServer();
        }

        private async void PingServer()
        {
            var userToken = await _localSettingsService.GetAuthToken();
            var firebaseToken = CrossFirebasePushNotification.Current.Token;
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

        private async void NavigateToContactScreen()
        {

            SetBusy(true);
            await NavigationService.PushAsync(new ManageContactsPage(), false);
            SetBusy(false);
        }

        private async void OpenSendAlertScreen(string alertType)
        {
            switch (alertType)
            {
                case "manual":
                    this.alertType = Model.AlertType.UserAlert;
                    break;
                case "fire":
                    this.alertType = Model.AlertType.Fire;
                    break;
                case "police":
                    this.alertType = Model.AlertType.Police;
                    break;
                case "health":
                    this.alertType = Model.AlertType.Health;
                    break;
            }
            SetBusy(true);
            ShowCancelButton = true;
            stop = false;            
            StartTimer();
        }
        private async void CancelSendAlert()
        {
            currentSecond = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                CancelButtonText = maxSeconds.ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
            });
            SetBusy(false);
            stop = true;
            ShowCancelButton = false;
        }

        private async void OpenSettingsScreen()
        {
            SetBusy(true);
            await NavigationService.PushAsync(new SettingsPage(), false);
            SetBusy(false);
        }

        private async void StartTimer()
        {
            for (int i = currentSecond; i < maxSeconds; i++)
            {
                if (stop)
                {
                    ShowCancelButton = false;
                    i = maxSeconds;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CancelButtonText = maxSeconds.ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
                    });
                    break;
                }

                if (!stop)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CancelButtonText = (maxSeconds - i).ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

            }

            if (!stop)
            {
                await NavigationService.PushAsync(new SendingAlertPage(alertType), false);
                SetBusy(false);
            }

            ShowCancelButton = false;
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {

            this.Busy = isBusy;
            ((Command)OpenContactsScreen).ChangeCanExecute();
            ((Command)SosCommand).ChangeCanExecute();

        }

        #endregion        
    }
}

