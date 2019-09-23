using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class AlertRespondPageViewModel : BaseViewModel
    {
        #region Properties
        private NotificationAction _notificationAction;

        private string _ContactName;

        public string ContactName
        {
            get { return _ContactName; }
            set
            {
                _ContactName = value;
                OnPropertyChanged("ContactName");
            }
        }


        private string _AlertTextTitle;

        public string AlertTextTitle
        {
            get { return _AlertTextTitle; }
            set
            {
                _AlertTextTitle = value;
                OnPropertyChanged("AlertTextTitle");
            }
        }

        #endregion

        #region Commands

        private ICommand _AcceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                return _AcceptCommand ?? (_AcceptCommand = new Command(Accept, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _IgnoreCommand;
        public ICommand IgnoreCommand
        {
            get
            {
                return _IgnoreCommand ?? (_IgnoreCommand = new Command(Ignore, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        #region Services
        private readonly ICryptographyService _cryptographyService;
        #endregion

        public AlertRespondPageViewModel(ICryptographyService cryptographyService, NotificationAction notificationAction)
        {
            _cryptographyService = cryptographyService;
            _notificationAction = notificationAction;
            SetProfileData();
        }

        private async void SetProfileData()
        {
            var data = _notificationAction.Data as AlertNotificationData;
            if (!string.IsNullOrWhiteSpace(data.ProfileData))
            {
                var profileData = await _cryptographyService.GetAlertSenderProfileData(data.ProfileData, data.FileKey);
                if (profileData.ContainsKey(RegistrationField.Name.FullName))
                {
                    ContactName = profileData[RegistrationField.Name.FullName];
                }
            }

            if (data.AlertType == (int)AlertType.UserAlert)
            {
                AlertTextTitle = "ALERT: " + AppResources.AlertSosTitle;
            }
        }

        private async void Accept()
        {
            var notifcationManager = DependencyService.Get<INotificationManager>();
            notifcationManager.CloseNotification(_notificationAction.NotificationId);
            await NavigationService.PopModalAsync();
        }
        private async void Ignore()
        {
            var notifcationManager = DependencyService.Get<INotificationManager>();
            notifcationManager.CloseNotification(_notificationAction.NotificationId);
            await NavigationService.PopModalAsync();
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
            ((Command)AcceptCommand).ChangeCanExecute();
            ((Command)IgnoreCommand).ChangeCanExecute();
        }

        #endregion 
    }
}
