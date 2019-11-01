using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Field> Fields { get; set; } = new ObservableCollection<Field>();

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
        private readonly IRegistrationService _registrationService;
        private readonly ILocalSettingsService _localSettingsService;
        IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        public AlertRespondPageViewModel(ICryptographyService cryptographyService, IRegistrationService registrationService, ILocalSettingsService localSettingsService, NotificationAction notificationAction)
        {
            _cryptographyService = cryptographyService;
            _registrationService = registrationService;
            _localSettingsService = localSettingsService;
            _notificationAction = notificationAction;
            _notificationManager = DependencyService.Get<INotificationManager>();
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            ProfileImage = ImageSource.FromFile("account_circle.png");
        }

        public async void SetProfileData()
        {
            //Fields.Add(new Field { Label = "Onoma", Value = "Thanos" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });
            //Fields.Add(new Field { Label = "Eponimo", Value = "Argyrakis" });            
            var data = _notificationAction.Data as AlertNotificationData;

            var token = await _localSettingsService.GetAuthToken();
            var registrationFiedsResult = await _registrationService.GetRegistrationFields(token);
            var tempFields = new List<Field>();
            if (registrationFiedsResult.IsOk)
            {

                var language = _localSettingsService.GetSelectedLanguage();
                foreach (var item in registrationFiedsResult.Result)
                {
                    if (item.Labels != null)
                    {
                        string label = "";
                        item.Labels.TryGetValue(language, out label);
                        tempFields.Add(new Field { Key = item.FieldName, Label = label });
                    }
                }
            }

            if (data.AlertType == (int)AlertType.UserAlert)
            {
                AlertTextTitle = "ALERT: " + AppResources.AlertSosTitle;
            }

            if (!string.IsNullOrWhiteSpace(data.ProfileData))
            {
                var profileData = await _cryptographyService.GetAlertSenderProfileData(data.ProfileData, data.FileKey);
                var name = profileData[RegistrationField.Names.Name];
                var surname = profileData[RegistrationField.Names.Surname];
                if (profileData != null)
                {
                    ContactPhone = data.Cellphone;
                    var addressBookContact = await GetContact(data.Cellphone);
                    if (addressBookContact != null)
                    {
                        ProfileImage = addressBookContact.ProfileImage;
                    }

                    if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
                        ContactName = String.Format("{0} {1}", profileData[RegistrationField.Names.Surname], profileData[RegistrationField.Names.Name]);
                    else
                    {
                        if (addressBookContact != null)
                        {
                            ContactName = addressBookContact.Name;
                        }
                    }

                    foreach (var item in profileData)
                    {
                        if (item.Key != RegistrationField.Names.Surname && item.Key != RegistrationField.Names.Name && !string.IsNullOrWhiteSpace(item.Value))
                        {
                            var registrationField = tempFields.Where(tf => tf.Key == item.Key).FirstOrDefault();
                            if (registrationField != null)
                            {
                                Fields.Add(new Field { Label = registrationField.Label, Value = item.Value });
                            }

                        }
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
                //here get contact from addressbook
                var contactService = DependencyService.Get<IContacts>();
                var addressBookContact = contactService.GetContactDetails(cellPhone);
                if (addressBookContact != null)
                {
                    return new ImportContact(addressBookContact, _contactProfileImageProvider);
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

        public class Field
        {
            public string Key { get; set; }
            public string Label { get; set; }
            public string Value { get; set; }
        }
    }
}
