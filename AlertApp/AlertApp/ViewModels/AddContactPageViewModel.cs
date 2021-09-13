using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Resx;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using Plugin.ContactService.Shared;
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

        public bool FabVisibile { get { return !Busy && this.Contacts != null && this.Contacts.Where(c => c.Selected).Count() > 0; } }

        public bool HasChange { get; set; }

        #endregion

        #region Commands

        private ICommand _InviteCommand;
        public ICommand InviteCommand
        {
            get
            {
                return _InviteCommand ?? (_InviteCommand = new Command(InviteUser, () =>
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

        public AddContactPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService, List<string> community)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            GetContacts(community);
        }

        private async void GetContacts(List<string> community)
        {

            var contactPermissionStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                var results = await Permissions.RequestAsync<Permissions.ContactsRead>();
                contactPermissionStatus = results;
            }

            if (contactPermissionStatus != PermissionStatus.Granted)
            {
                showOKMessage("Permissions Denied", "Unable get contacts.");
                return;
            }
            SetBusy(true);

            //var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            var contacts = await Xamarin.Essentials.Contacts.GetAllAsync();
            if (contacts != null)
            {
                var tempList = new List<ImportContact>();
                var filteredContacts = contacts.Where(c => c.Phones.Count > 0).OrderBy(c => c.FamilyName).ToList();
                foreach (var item in filteredContacts)
                {
                    foreach (var number in item.Phones)
                    {
                        var num = ImportContact.GetFormattedNumber(number.PhoneNumber);
                        if (!community.Contains(num))
                            tempList.Add(new ImportContact(item, num, _contactProfileImageProvider));
                    }
                    
                }

                //if (Device.RuntimePlatform == Device.iOS)
                //{
                //    for (int i = 0; i < tempList.Count; i++)
                //    {
                //        if (tempList[i]?.Number == null) continue;
                //        var spl1 = tempList[i].Number.Split(':');
                //        var spl2 = spl1[2].Split(',');
                //        var spl3 = spl2[0].Split('=');
                //        tempList[i].Number = spl3[1];
                //    }
                //}

                this.Contacts = new ObservableCollection<ImportContact>();
                this.OriginalContacts = new ObservableCollection<ImportContact>(this.Contacts);

                //call service to find which number is app user
                var serverContacts = await _contactsService.CheckContacts(await _localSettingsService.GetAuthToken(), tempList.Select(c => c.FormattedNumber).ToArray());

                if (serverContacts != null && serverContacts.IsOk)
                {
                    //get contacts where not app user
                    var serverContactsWithApp = serverContacts.Result.Contacts.Where(x => x.Value == true).Select(x => x.Key).ToList();

                    var userContactsWithApp = tempList.Where(c => serverContactsWithApp.Contains(c.FormattedNumber)).ToList();
                    userContactsWithApp.ForEach(c => c.NeedsInvitation = false);

                    this.Contacts = new ObservableCollection<ImportContact>(userContactsWithApp);
                    this.OriginalContacts = new ObservableCollection<ImportContact>(this.Contacts);
                }
                else if (!serverContacts.IsOnline)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        this.OriginalContacts = new ObservableCollection<ImportContact>();
                        this.Contacts = new ObservableCollection<ImportContact>(this.Contacts); ;
                    });

                    showOKMessage(AppResources.Error, AppResources.NoInternetConnection);
                }

            }
            SetBusy(false);
        }

        private async void InviteUser()
        {
            string messageText = AppResources.ShareMessage;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = AppResources.ShareMessage,
                Title = AppResources.ShareMessageTitle
            });
        }
        public void SelectContact(ImportContact contact)
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
                        HasChange = true;
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else if (addContactsResults.IsOnline)
                    {
                        showOKMessage(AppResources.Error, AppResources.NoInternetConnection);
                    }
                    SetBusy(false);
                }
            }
        }

        private void FilterContacts(string searchText)
        {
            string searchtext = !string.IsNullOrWhiteSpace(searchText) ? searchText.ToLower() : null;

            searchtext = Language.RemoveDiacritics(searchtext);
            if (this.OriginalContacts != null && searchtext != null)
            {

                var filtered = this.OriginalContacts.
                    Where(
                    (sc => sc.Name != null && sc.NormalizedName.ToLower().Contains(searchtext)
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
            var mobileNumber = await dialogs.showInputDialog(AppResources.EnterMobileDialogTitle, AppResources.EnterMobileDialogMessage, AppResources.OK, AppResources.Cancel, DialogType.Phone);
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
