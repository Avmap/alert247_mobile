using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Services.Alert;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        #endregion

        public SendingAlertPageViewModel(IAlertService alertService, ILocalSettingsService localSettingsService, AlertType alertType)
        {
            _alertService = alertService;
            _localSettingsService = localSettingsService;
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
            var sendOK = await _alertService.SendAlert("userid");
            if (sendOK)
            {
                await NavigationService.PopAsync();
            }
            SetBusy(false);
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
