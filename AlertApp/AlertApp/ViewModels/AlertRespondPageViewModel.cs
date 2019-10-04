using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AlertApp.ViewModels
{
    public class AlertRespondPageViewModel : BaseViewModel
    {
        #region Properties

        private ImageSource _ProfileImage;

        public ImageSource ProfileImage
        {
            get { return _ProfileImage; }
            set
            {
                _ProfileImage = value;
                OnPropertyChanged("ProfileImage");
            }
        }


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
        private string _ContactPhone;
        public string ContactPhone
        {
            get { return _ContactPhone; }
            set
            {
                _ContactPhone = value;
                OnPropertyChanged("ContactPhone");
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

        private Position _Position;

        public Position Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                OnPropertyChanged("Position");
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
        private readonly INotificationManager _notificationManager;
        IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        public AlertRespondPageViewModel(ICryptographyService cryptographyService, NotificationAction notificationAction)
        {
            _cryptographyService = cryptographyService;
            _notificationAction = notificationAction;
            _notificationManager = DependencyService.Get<INotificationManager>();
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            ProfileImage = ImageSource.FromFile("account_circle.png");
        }

        public async void SetProfileData()
        {
            var data = _notificationAction.Data as AlertNotificationData;


            if (data.AlertType == (int)AlertType.UserAlert)
            {
                AlertTextTitle = "ALERT: " + AppResources.AlertSosTitle;
            }

            if (!string.IsNullOrWhiteSpace(data.ProfileData))
            {
                var profileData = await _cryptographyService.GetAlertSenderProfileData(data.ProfileData, data.FileKey);
                if (profileData != null && profileData.ContainsKey(RegistrationField.Name.FullName))
                {
                    ContactName = profileData[RegistrationField.Name.FullName];
                    var addressBookContact = await GetContact(data.Cellphone);
                    if (addressBookContact != null)
                    {
                        ProfileImage = addressBookContact.ProfileImage;
                    }
                }
                else if (profileData == null)
                {
                    if (!string.IsNullOrWhiteSpace(data.Cellphone))
                    {
                        SetContactFromMobile(data.Cellphone);
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        _notificationManager.CloseNotification(_notificationAction.NotificationId);
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                }
            }
            else
            {
                //here get contact from addressbook
                SetContactFromMobile(data.Cellphone);
            }

        }

        private async void SetContactFromMobile(string cellphone)
        {
            var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (contactPermissionStatus == PermissionStatus.Granted)
            {

                //here get contact from addressbook
                var contactService = DependencyService.Get<IContacts>();
                var addressBookContact = contactService.GetContactDetails(cellphone);
                if (addressBookContact != null)
                {
                    ContactName = addressBookContact.FirstName;
                    if (!string.IsNullOrWhiteSpace(addressBookContact.ProfileImageUri))
                    {
                        var image = _contactProfileImageProvider.GetProfileImage(addressBookContact.ProfileImageUri);
                        ProfileImage = image;
                    }
                    else
                    {
                        ProfileImage = ImageSource.FromFile("account_circle.png");
                    }
                    ContactPhone = cellphone;
                }
            }
            else
            {
                ContactPhone = cellphone;
                ProfileImage = ImageSource.FromFile("account_circle.png");
            }
        }

        private async void Accept()
        {
            _notificationManager.CloseNotification(_notificationAction.NotificationId);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
        private async void Ignore()
        {
            _notificationManager.CloseNotification(_notificationAction.NotificationId);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task<ImportContact> GetContact(string cellPhone)
        {
            var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (contactPermissionStatus == PermissionStatus.Granted)
            {
                var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                if (contacts != null)
                {
                    var result = new List<ImportContact>();
                    var contact = contacts.Where(c => c.Number == cellPhone).FirstOrDefault();
                    if (contact != null)
                    {
                        return new ImportContact(contact, _contactProfileImageProvider);
                    }
                }
            }

            return null;
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
