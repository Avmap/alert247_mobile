using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
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
    public class MyCommunityPageViewModel : BaseViewModel, IHaveContacts
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
            SetBusy(true);
        }

        private void GetCommunity()
        {
            SetBusy(true);
            MessagingCenter.Send((BaseViewModel)this, RefreshContactsEvent.Event, new RefreshContactsEvent { });
        }

        private async void SetCommunity(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            if (response != null && response.IsOk)
            {
                var community = response.Result.Contacts.Community;
                if (community != null && community.Count > 0)
                {
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        Community.Clear();
                        foreach (var item in community)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();
                            if (addressBookItem != null)
                            {
                                Community.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                Community.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }
                        SetBusy(false);
                    }
                    else
                    {
                        var contacts = community.Select(c => new Contact { Accepted = c.Accepted, Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        Community.Clear();
                        foreach (var item in contacts)
                        {
                            Community.Add(item);
                        }
                        SetBusy(false);
                    }
                }
                else
                {
                    Community.Clear();
                    SetBusy(false);
                }
            }
            else
            {
                SetBusy(false);
            }
            OnPropertyChanged("HasContacts");

        }
        private async void OpenAddContactsPage()
        {
            var contactsPage = new AddContactPage();
            contactsPage.Disappearing += (sender2, e2) =>
            {
                SetBusy(false);
                var vm = contactsPage.BindingContext as AddContactPageViewModel;
                if (vm.HasChange)
                {
                    foreach (var page in NavigationService.NavigationStack)
                    {
                        if (page is ManageContactsPage)
                        {
                            SetBusy(true);
                            ((ManageContactsPage)page).RefreshContacts();
                            break;
                        }
                    }
                }
            };
            await NavigationService.PushAsync(contactsPage, true);
        }



        public async Task<bool> RemoveUser(Contact contact)
        {
            SetBusy(true);
            List<string> contacts = new List<string>();
            contacts.Add(contact.Cellphone);
            var response = await _contactsService.RemoveContacts(await _localSettingsService.GetAuthToken(), contacts);
            if (response.IsOk)
            {
                GetCommunity();
            }
            else
            {
                SetBusy(false);
            }
            return true;
        }
        public async Task<bool> BlockUser(Contact contact)
        {
            SetBusy(true);
            var response = await _contactsService.BlockAdd(await _localSettingsService.GetAuthToken(), contact.Cellphone);
            if (response.IsOk)
            {
                GetCommunity();
            }
            else
            {
                SetBusy(false);
            }

            return true;
        }
        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                ((Command)OpenContactsScreenCommand).ChangeCanExecute();
            });
        }
        #endregion

        #region IHaveContacts
        public void SetContacts(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            SetCommunity(response, addressBook);
        }

        #endregion

    }
}
