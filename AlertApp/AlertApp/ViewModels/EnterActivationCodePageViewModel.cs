using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class EnterActivationCodePageViewModel : BaseViewModel
    {
        #region Services
        readonly IRegistrationService _registrationService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties

        string[] Code = new string[] { "", "", "", "", "", "" };

        private string _MobileNumber { get; set; }

        private string _VerificationCode;


        public string VerificationCode
        {
            get
            {
                if (_VerificationCode == null)
                    _VerificationCode = String.Empty;

                return _VerificationCode;
            }
            set
            {
                _VerificationCode = value;
                if (_VerificationCode.Length == 6)
                {
                    for (int i = 0; i < _VerificationCode.Length; i++)
                    {
                        Code[i] = _VerificationCode.Substring(i, 1);
                    }
                }
                OnPropertyChanged("VerificationCode");
                OnPropertyChanged("VerificationCode0");
                OnPropertyChanged("VerificationCode1");
                OnPropertyChanged("VerificationCode2");
                OnPropertyChanged("VerificationCode3");
                OnPropertyChanged("VerificationCode4");
                OnPropertyChanged("VerificationCode5");
                OnPropertyChanged("VerificationCode6");
                ((Command)ContinueCommand).ChangeCanExecute();
            }
        }

        private string _TimerText;

        public string TimerText
        {
            get { return _TimerText; }
            set
            {
                _TimerText = value;
                OnPropertyChanged("TimerText");
            }
        }

        #region VerificationCodes
        public string VerificationCode0
        {
            get { return !string.IsNullOrWhiteSpace(Code[0]) ? Code[0] : ""; }
            set
            {
                Code[0] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }

        public string VerificationCode1
        {
            get { return !string.IsNullOrWhiteSpace(Code[1]) ? Code[1] : ""; }
            set
            {
                Code[1] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }

        public string VerificationCode2
        {
            get { return !string.IsNullOrWhiteSpace(Code[2]) ? Code[2] : ""; }
            set
            {
                Code[2] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }

        public string VerificationCode3
        {
            get { return !string.IsNullOrWhiteSpace(Code[3]) ? Code[3] : ""; }
            set
            {
                Code[3] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }
        public string VerificationCode4
        {
            get { return !string.IsNullOrWhiteSpace(Code[4]) ? Code[4] : ""; }
            set
            {
                Code[4] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }
        public string VerificationCode5
        {
            get { return !string.IsNullOrWhiteSpace(Code[5]) ? Code[5] : ""; }
            set
            {
                Code[5] = value;
                VerificationCode = String.Concat(Code);
                OnPropertyChanged("CanContinue");
            }
        }

        public bool CanContinue => !string.IsNullOrWhiteSpace(VerificationCode) && VerificationCode.Length == 6;
        #endregion

        public bool CanEditCode => !Busy;

        private bool _CanResendCode;

        public bool CanResendCode
        {
            get { return _CanResendCode; }
            set
            {
                _CanResendCode = value;
                OnPropertyChanged("RefreshImageSource");
                OnPropertyChanged("CanResendCode");
                OnPropertyChanged("CanContinue");
                ((Command)ResendCodeCommand).ChangeCanExecute();
            }
        }

        public ImageSource RefreshImageSource => _CanResendCode ? ImageSource.FromFile("refresh.png") : ImageSource.FromFile("refresh_grey.png");
        public Color EnabledDisabledColor => _CanResendCode ? Color.Black : Color.Gray;

        private Timer _smsTimer;

        private int timerRunTimes;

        private int _countDownSeconds = 59;
        #endregion

        #region Commands

       

        private ICommand _ResendCodeCommand;
        public ICommand ResendCodeCommand
        {
            get
            {
                return _ResendCodeCommand ?? (_
                    = new Command(RequestVerificationCode, () =>
                {
                    return CanResendCode;
                }));
            }
        }

        private ICommand _ContinueCommand;
        public ICommand ContinueCommand
        {
            get
            {
                return _ContinueCommand ?? (_ContinueCommand = new Command(Continue, () =>
                {
                    return !string.IsNullOrWhiteSpace(VerificationCode) && VerificationCode.Length == 6;
                }));
            }
        }
        #endregion

        public EnterActivationCodePageViewModel(IRegistrationService registrationService, ILocalSettingsService localSettingsService, string mobilenumber)
        {
            _registrationService = registrationService;
            _localSettingsService = localSettingsService;
            _MobileNumber = mobilenumber;

            //_smsTimer = new System.Timers.Timer();
            //_smsTimer.Interval = 1000;
            //_smsTimer.Elapsed += OnTimedEvent;
//#if Release
            RequestVerificationCode();
//#endif

        }

        public void RegisterForSmsEvent()
        {
            MessagingCenter.Subscribe<IOtpMessageNotifier, OtpMessageReceivedEvent>(this, OtpMessageReceivedEvent.Event, (sender, arg) =>
            {
                if (arg != null && !string.IsNullOrWhiteSpace(arg.VerificationMessage))
                {
                    VerificationCode = GetCode(arg.VerificationMessage);
                }
            });
        }

        public void UnRegisterForSmsEvent()
        {
            MessagingCenter.Unsubscribe<IOtpMessageNotifier, OtpMessageReceivedEvent>(this, OtpMessageReceivedEvent.Event);
        }

        private string GetCode(string message)
        {
            int lineBreakPosition = message.IndexOf('\n');
            var code = message.Substring(lineBreakPosition - 6, 6);
            return code;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (timerRunTimes > 50)
                    TimerText = "00:0" + (_countDownSeconds - timerRunTimes);
                else
                    TimerText = "00:" + (_countDownSeconds - timerRunTimes);

                if (timerRunTimes == _countDownSeconds)
                {
                    _smsTimer.Stop();
                    CanResendCode = true;
                    timerRunTimes = 0;
                    return;
                }

                timerRunTimes++;
            });
        }

       
        private async void RequestVerificationCode()
        {
            SetBusy(true);
            string applicationHash = "FoH283gIlH0";
            //if device is android , start sms retreiver to listen for otp sms message
            if (Device.RuntimePlatform == Device.Android)
            {
                var otpVerificationService = DependencyService.Get<IOtpVerification>();
                if (otpVerificationService != null)
                {
                    otpVerificationService.StartSmsRetriever();
                    applicationHash = otpVerificationService.GetApplicationHash();
                }
            }
            var response = await _registrationService.Register(_MobileNumber, _localSettingsService.GetSelectedLanguage(), applicationHash);

            if (response.IsOk)
            {
                //_smsTimer.Start();
                CanResendCode = false;
            }

            if (!response.IsOk && response.ErrorDescription != null && response.ErrorDescription.Labels != null)
            {
                showOKMessage(AppResources.Error, GetErrorDescription(response.ErrorDescription.Labels));

                //_smsTimer.Stop();
                CanResendCode = true;
            }
            else if (!response.IsOk && !response.IsOnline)
            {
                showOKMessage(AppResources.Error, AppResources.NoInternetConnection);
                //ResetCounter();
            }
            SetBusy(false);
        }

        private async void Continue()
        {
            OnPropertyChanged("CanContinue");
            SetBusy(true);
            var response = await _registrationService.ConfirmRegistration(_MobileNumber, VerificationCode);
            if (response != null && response.Result != null && !string.IsNullOrWhiteSpace(response.Status))
            {
                _localSettingsService.SaveAuthToken(response.Result.Token);
                _localSettingsService.SaveUserId(response.Result.UserID);
                await _localSettingsService.SaveMobilePhone(_MobileNumber);
                App.TempRegistrationFields = response.Result.Fields;
                await Application.Current.MainPage.Navigation.PushAsync(new EnterApplicationPinCodePage(), false);
            }
            else
            {
                if (!response.IsOk && response.ErrorDescription != null && response.ErrorDescription.Labels != null)
                {
                    showOKMessage(AppResources.Error, GetErrorDescription(response.ErrorDescription.Labels));
                }

            }
            SetBusy(false);
        }

        private void ResetCounter()
        {
            CanResendCode = true;
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                OnPropertyChanged("CanEditCode");
            });
        }
        #endregion

    }
}
