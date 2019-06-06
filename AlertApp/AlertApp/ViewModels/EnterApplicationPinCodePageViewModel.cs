using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
                SetBusy(true);
                await Task.Run(() => _cryptohraphyService.GenerateKeys(Pin));                             
                SetBusy(false);
                //we keep TempRegistrationFields in static field in App.xaml.cs.
                await NavigationService.PushAsync(new RegistrationFieldsPage(App.TempRegistrationFields), true);
            }
        }

        private void GenerateKeys()
        {
            var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            _localSettingsService.SaveApplicationPin(Pin);
            ICryptographicKey key = asym.CreateKeyPair(4096);

            var publicKey = key.ExportPublicKey();
            var privateKey = key.Export();

            var publicKeyString = Convert.ToBase64String(publicKey);
            var privateKeyString = Convert.ToBase64String(privateKey);
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
