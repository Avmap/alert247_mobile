using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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


        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new Command(CancelSendAlert, () =>
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
            }
        }

        volatile bool stop = false;
        volatile int seconds = 5;
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
        }

        private async void OpenSendAlertScreen()
        {
            ShowCancelButton = true;
            stop = false;
            StartTimer();
        }
        private async void CancelSendAlert()
        {
            stop = true;
        }

        private async void StartTimer()
        {
            for (int i = 0; i < seconds; i++)
            {
                if (!stop)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CancelButtonText = AppResources.CancelSendAlert + "\n" + (seconds - i);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                if (stop)
                {
                    ShowCancelButton = false;
                    break;
                }

            }

            if (!stop)
            {
                await NavigationService.PushAsync(new SendingAlertPage(Model.AlertType.UserAlert), true);
            }

            ShowCancelButton = false;
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion        
    }
}

