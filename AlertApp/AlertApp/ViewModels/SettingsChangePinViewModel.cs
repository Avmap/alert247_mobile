using AlertApp.Infrastructure;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SettingsChangePinViewModel : BaseViewModel
    {
        #region Services        
        readonly ILocalSettingsService _localSettingsService;
        readonly ICryptographyService _cryptographyService;
        readonly IUserProfileService _userProfileService;
        #endregion

        #region Properties
        private bool _NewPinLayoutVisible;

        public bool NewPinLayoutVisible
        {
            get { return _NewPinLayoutVisible; }
            set
            {
                _NewPinLayoutVisible = value;
                OnPropertyChanged("CurrentPinLayoutVisible");
                OnPropertyChanged("NewPinLayoutVisible");
            }
        }

        private bool _CanFinish;

        public bool CanFinish
        {
            get { return _CanFinish; }
            set
            {
                _CanFinish = value;
                OnPropertyChanged("CanFinish");                
            }
        }

        public string VePin1 { get; set; } 
        public string VePin2 { get; set; }
        public string VePin3 { get; set; }
        public string VePin4 { get; set; } 
        #endregion

        #region Commands

        private ICommand _ChangePinCommand;
        public ICommand ChangePinCommand
        {
            get
            {
                return _ChangePinCommand ?? (_ChangePinCommand = new Command(ChangePin, () =>
                {
                    return !Busy;
                }));
            }
        }
  
        
        #endregion


        public bool CurrentPinLayoutVisible => !NewPinLayoutVisible;

        public SettingsChangePinViewModel(ILocalSettingsService localSettingsService, ICryptographyService cryptographyService, IUserProfileService userProfileService)
        {
            _localSettingsService = localSettingsService;
            _cryptographyService = cryptographyService;
            _userProfileService = userProfileService;
        }

        public Task<string> GetApplicationPin()
        {
            return _localSettingsService.GetApplicationPin();
        }

        private async void ChangePin()
        {
            string newPin = String.Format("{0}{1}{2}{3}", VePin1, VePin2, VePin3, VePin4);
            var changed = await _cryptographyService.ChangePin(newPin);
            if (changed)
            {
                SetBusy(true);
                try
                {
                    var storedProfile = await _userProfileService.StoreProfile(new Dictionary<string, string>(), await _localSettingsService.GetAuthToken(), await _localSettingsService.GetPublicKey());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                SetBusy(false);

                showOKMessage(AppResources.Succcess, AppResources.SucccessChangePinMessage);
                await Application.Current.MainPage.Navigation.PopAsync(false);
            }
        }

        public override void SetBusy(bool isBusy)
        {

        }
    }
}
