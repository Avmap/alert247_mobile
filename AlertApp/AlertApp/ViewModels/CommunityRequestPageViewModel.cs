using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class CommunityRequestPageViewModel : BaseViewModel
    {

        #region Commands
        private ICommand _AcceptRequestCommand;
        public ICommand AcceptRequestCommand
        {
            get
            {
                return _AcceptRequestCommand ?? (_AcceptRequestCommand = new Command(AcceptRequest, () =>
                {
                    return !Busy;
                }));
            }
        }
        private ICommand _IgnoreRequestCommand;
        public ICommand IgnoreRequestCommand
        {
            get
            {
                return _IgnoreRequestCommand ?? (_IgnoreRequestCommand = new Command(IgnoreRequest, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _BlockRequestCommand;
        public ICommand BlockRequestCommand
        {
            get
            {
                return _BlockRequestCommand ?? (_BlockRequestCommand = new Command(BlockRequest, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        readonly INotificationManager _notificationManager;
        #endregion

        #region Properties        
        private int _notificationId;
        private Contact _contact;

        public Contact Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                OnPropertyChanged("Contact");
            }
        }

        public bool HasChange { get; set; }

        #endregion

        public CommunityRequestPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService, Contact contact)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _notificationManager = DependencyService.Get<INotificationManager>();
            
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            _notificationId = contact.NotificationId;
            var contactService = DependencyService.Get<IContacts>();
            var addressBookContact = contactService.GetContactDetails(contact.Cellphone);
            Contact = contact;
            if (addressBookContact != null && !string.IsNullOrWhiteSpace(addressBookContact.ProfileImageUri))
            {
                var image = _contactProfileImageProvider.GetProfileImage(addressBookContact.ProfileImageUri);
                Contact.ProfileImage = image;
                Contact.FirstName = addressBookContact.FirstName;
            }

            if (_notificationId == 0)
            {
                _notificationId = _localSettingsService.GetCellPhoneNotificationId(contact.Cellphone);     
            }
        }

        private async void AcceptRequest()
        {
            SetBusy(true);
            var result = await _contactsService.AcceptAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }
            SetBusy(false);
            if (_notificationId != 0)
                _notificationManager.CloseNotification(_notificationId);
            await NavigationService.PopModalAsync();
        }

        private async void BlockRequest()
        {
            SetBusy(true);
            var result = await _contactsService.BlockAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }
            SetBusy(false);
            if (_notificationId != 0)
                _notificationManager.CloseNotification(_notificationId);
            await NavigationService.PopModalAsync();
        }

        private async void IgnoreRequest()
        {
            SetBusy(true);
            var result = await _contactsService.IgnoreAdd(await _localSettingsService.GetAuthToken(), _contact.Cellphone);
            if (result.IsOk && result.Result == true)
            {
                HasChange = true;
            }
            SetBusy(false);
            if (_notificationId != 0)
                _notificationManager.CloseNotification(_notificationId);
            await NavigationService.PopModalAsync();
        }
        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)AcceptRequestCommand).ChangeCanExecute();
                ((Command)IgnoreRequestCommand).ChangeCanExecute();
                ((Command)BlockRequestCommand).ChangeCanExecute();
            });
        }
        #endregion
    }
}
