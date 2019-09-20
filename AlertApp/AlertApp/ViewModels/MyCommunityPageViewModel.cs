using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Community;
using AlertApp.Services.Contacts;
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

namespace AlertApp.ViewModels
{
    public class MyCommunityPageViewModel : BaseViewModel
    {
        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        #region Properties
        private ObservableCollection<Contact> _Community;
        public ObservableCollection<Contact> Community
        {
            get
            {
                if (_Community == null)
                {
                    _Community = new ObservableCollection<Contact>();
                }
                return _Community;
            }
            set
            {
                _Community = value;
                OnPropertyChanged("Community");
                OnPropertyChanged("HasContacts");
            }
        }

        public bool HasContacts => Community == null || Community.Count == 0;

        #endregion

        #region Commands
        private ICommand _OpenContactsScreenCommand;
        public ICommand OpenContactsScreenCommand
        {
            get
            {
                return _OpenContactsScreenCommand ?? (_OpenContactsScreenCommand = new Command(OpenAddContactsPage, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _GetCommunityCommand;
        public ICommand GetCommunityCommand
        {
            get
            {
                return _GetCommunityCommand ?? (_GetCommunityCommand = new Command(GetCommunity, () =>
                {
                    return !Busy;
                }));
            }
        }

        #endregion
        public MyCommunityPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            GetCommunity();
        }

        private async void GetCommunity()
        {
            SetBusy(true);
            var token = await _localSettingsService.GetAuthToken();
            var webServiceContacts = await _contactsService.GetContacts(token);
            if (webServiceContacts != null && webServiceContacts.IsOk)
            {
                var community = webServiceContacts.Result.Contacts.Community;
                if (community != null && community.Count > 0)
                {
                    var addressBook = await GetAddressbook();
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        Community.Clear();
                        foreach (var item in community)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();
                            if (addressBookItem != null)
                            {
                                Community.Add(new Contact { Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                Community.Add(new Contact { Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }
                    }
                    else
                    {
                        var contacts = community.Select(c => new Contact { Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        Community.Clear();
                        foreach (var item in contacts)
                        {
                            Community.Add(item);
                        }
                    }
                }

            }
            OnPropertyChanged("HasContacts");
            SetBusy(false);
        }
        private async void OpenAddContactsPage()
        {
            await NavigationService.PushAsync(new AddContactPage(), true);
        }

        private async Task<List<ImportContact>> GetAddressbook()
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
                return null;
            }
            SetBusy(true);

            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            if (contacts != null)
            {
                var result = new List<ImportContact>();
                foreach (var item in contacts.Where(c => c.Number != null).OrderBy(c => c.Name))
                {
                    result.Add(new ImportContact(item, _contactProfileImageProvider));
                }
                return result;

            }

            return null;
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)OpenContactsScreenCommand).ChangeCanExecute();
                ((Command)GetCommunityCommand).ChangeCanExecute();
            });
        }
        #endregion
    }
}
