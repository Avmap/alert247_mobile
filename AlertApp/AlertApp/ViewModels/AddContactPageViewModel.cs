using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using Plugin.ContactService.Shared;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class AddContactPageViewModel : BaseViewModel
    {
        #region Properties
        private string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                _SearchText = value;
                OnPropertyChanged("SearchText");
                if (string.IsNullOrWhiteSpace(value))
                {
                    this.Contacts = new ObservableCollection<ImportContact>(OriginalContacts);
                }
                else
                {
                    FilterContacts(value);
                }
            }
        }

        private ObservableCollection<ImportContact> _Contacts;
        public ObservableCollection<ImportContact> Contacts
        {
            get { return _Contacts; }
            set
            {
                _Contacts = value;
                OnPropertyChanged("Contacts");
                OnPropertyChanged("FabVisibile");
            }
        }
        private ObservableCollection<ImportContact> _OriginalContacts;
        public ObservableCollection<ImportContact> OriginalContacts
        {
            get { return _OriginalContacts; }
            set
            {
                _OriginalContacts = value;
                OnPropertyChanged("Original");
            }
        }

        public bool FabVisibile { get { return !Busy && this.Contacts.Where(c => c.Selected).Count() > 0; } }
        #endregion

        #region Commands

        private ICommand _InviteCommand;
        public ICommand InviteCommand
        {
            get
            {
                return _InviteCommand ?? (_InviteCommand = new Command<ImportContact>(InviteUser, (contact) =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _SelectContactCommand;
        public ICommand SelectContactCommand
        {
            get
            {
                return _SelectContactCommand ?? (_SelectContactCommand = new Command<ImportContact>(SelectContact, (contact) =>
                {
                    return !Busy;
                }));
            }
        }
        private ICommand _AddContactsCommand;
        public ICommand AddContactsCommand
        {
            get
            {
                return _AddContactsCommand ?? (_AddContactsCommand = new Command(AddContacts, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _FilterContactsCommand;

        public ICommand FilterContactsCommand
        {
            get
            {
                return _FilterContactsCommand ?? (_FilterContactsCommand = new Command<string>(FilterContacts, (searchText) =>
                {
                    return !Busy;
                }));
            }

        }

        private ICommand _EnterNumberCommand;

        public ICommand EnterNumberCommand
        {
            get
            {
                return _EnterNumberCommand ?? (_EnterNumberCommand = new Command(EnterNumber, () =>
                {
                    return !Busy;
                }));
            }

        }

        #endregion

        #region Services
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        public AddContactPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            GetContacts();
        }

        private async void GetContacts()
        {


            var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Contacts });
                contactPermissionStatus = results[Permission.Contacts];
            }

            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                showOKMessage("Permissions Denied", "Unable get contacts.");
                return;
            }
            SetBusy(true);

            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            if (contacts != null)
            {
                Contacts = new ObservableCollection<ImportContact>();
                foreach (var item in contacts.Where(c => c.Number != null).OrderBy(c => c.Name))
                {
                    Contacts.Add(new ImportContact(item, _contactProfileImageProvider));
                }
                this.OriginalContacts = new ObservableCollection<ImportContact>(this.Contacts);

                //call service to find which number is app user
                var serverContacts = await _contactsService.CheckContacts(await _localSettingsService.GetAuthToken(), this.OriginalContacts.Select(c => c.FormattedNumber).ToArray());

                //get contacts where not app user
                var serverContactsNeedInvitation = serverContacts.Result.Contacts.Where(x => x.Value == false).Select(x => x.Key).ToList();

                var needInvitationContacts = this.OriginalContacts.Where(c => serverContactsNeedInvitation.Contains(c.FormattedNumber)).ToList();
                needInvitationContacts.ForEach(c => c.NeedsInvitation = true);

            }
            SetBusy(false);
        }

        private async void InviteUser(ImportContact contact)
        {
            string messageText = String.Format("Download Alert247 {0}", "https://play.google.com/store/apps/details?id=gr.avmap.alert247");
            string action = await DisplayActionSheet(AppResources.ShareVia, new string[] { "SMS", AppResources.OtherText });
            if (action == "SMS")
            {
                try
                {
                    var message = new SmsMessage(messageText, new[] { contact.Number });
                    await Sms.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Sms is not supported on this device.
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }
            }
            else if (action == AppResources.OtherText)
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = "https://play.google.com/store/apps/details?id=gr.avmap.alert247",
                    Title = "Get Alert 24/7 app",
                });
            }
        }
        private void SelectContact(ImportContact contact)
        {
            if (!contact.NeedsInvitation)
            {
                contact.Selected = !contact.Selected;
                OnPropertyChanged("Contacts");
                OnPropertyChanged("FabVisibile");
            }

        }
        private async void AddContacts()
        {
            if (this.Contacts.Where(c => c.Selected).Count() > 0)
            {
                var ok = await showAlertMessage(AppResources.AlertAddContactsTitle, AppResources.AlertAddContactsMessage, AppResources.OK, AppResources.Cancel);
                if (ok == true)
                {
                    SetBusy(true);
                    var userToken = await _localSettingsService.GetAuthToken();
                    var addContactsResults = await _contactsService.AddContacts(userToken, this.Contacts.Where(c => c.Selected).Select(c => c.FormattedNumber).ToArray());
                    if (addContactsResults.IsOk)
                    {

                    }
                    SetBusy(false);
                }
            }
        }

        private void FilterContacts(string searchText)
        {
            string searchtext = !string.IsNullOrWhiteSpace(searchText) ? searchText.ToLower() : null;
            if (this.OriginalContacts != null && searchtext != null)
            {

                var filtered = this.OriginalContacts.
                    Where(
                    (sc => sc.Name != null && sc.Name.ToLower().Contains(searchtext)
                    || sc.Number != null && sc.Number.ToLower().Contains(searchText)));
                this.Contacts = new ObservableCollection<ImportContact>(filtered);
            }
            else
            {
                if (searchtext == null)
                    this.Contacts = new ObservableCollection<ImportContact>(OriginalContacts);
            }
        }
        private async void EnterNumber()
        {
            var dialogs = DependencyService.Get<IDialog>();
            var mobileNumber = await dialogs.showInputDialog(AppResources.EnterMobileDialogTitle, AppResources.EnterMobileDialogMessage, DialogType.Phone);
            if (!string.IsNullOrWhiteSpace(mobileNumber as string))
            {
                SetBusy(true);
                var userToken = await _localSettingsService.GetAuthToken();
                var addContactsResults = await _contactsService.AddContacts(userToken, new string[] { mobileNumber as string });
                if (addContactsResults.IsOk)
                {
                    showOKMessage(AppResources.SuccessAddContactTitle, AppResources.SuccessAddContactMessage);
                }
                else
                {
                    if (!addContactsResults.IsOnline)
                    {

                    }
                    else
                    {

                    }
                }
                SetBusy(false);
            }

        }


        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
            ((Command)FilterContactsCommand).ChangeCanExecute();
            ((Command)EnterNumberCommand).ChangeCanExecute();
        }

        #endregion 
    }
}
