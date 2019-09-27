using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class BlockedUsersPageViewModel : BaseViewModel, IHaveContacts
    {

        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        #region Commands

        private ICommand _GetBlockedContactsCommand;
        public ICommand GetBlockedContactsCommand
        {
            get
            {
                return _GetBlockedContactsCommand ?? (_GetBlockedContactsCommand = new Command(GetBlockedContacts, () =>
                {
                    return !Busy;
                }));
            }
        }

        #endregion

        #region Properties
        private ObservableCollection<Contact> _BlockedContacts;
        public ObservableCollection<Contact> BlockedContacts
        {
            get
            {
                if (_BlockedContacts == null)
                {
                    _BlockedContacts = new ObservableCollection<Contact>();
                }
                return _BlockedContacts;
            }
            set
            {
                _BlockedContacts = value;
                OnPropertyChanged("BlockedContacts");
                OnPropertyChanged("HasContacts");
            }
        }

        public bool HasContacts => BlockedContacts == null || BlockedContacts.Count == 0;

        #endregion

        public BlockedUsersPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            SetBusy(true);
        }

        private void GetBlockedContacts()
        {
            SetBusy(true);
            MessagingCenter.Send((BaseViewModel)this, RefreshContactsEvent.Event, new RefreshContactsEvent { });
        }

        private async void SetBlockedContacts(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            if (response != null && response.IsOk)
            {
                var blocked = response.Result.Contacts.Blocked;
                if (blocked != null && blocked.Count > 0)
                {                    
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        BlockedContacts.Clear();
                        foreach (var item in blocked)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();
                            if (addressBookItem != null)
                            {
                                BlockedContacts.Add(new Contact { ProfileImageUri = addressBookItem.PhotoUri, Accepted = item.Accepted, Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                BlockedContacts.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }
                        SetBusy(false);
                    }
                    else
                    {
                        var contacts = blocked.Select(c => new Contact { Accepted = c.Accepted, Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        BlockedContacts.Clear();
                        foreach (var item in contacts)
                        {
                            BlockedContacts.Add(item);
                        }
                        SetBusy(false);
                    }
                }
                else
                {
                    BlockedContacts.Clear();
                    SetBusy(false);
                }

            }
            else
            {
                SetBusy(false);
            }
            OnPropertyChanged("HasContacts");
            SetBusy(false);
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
              //  ((Command)_GetBlockedContactsCommand).ChangeCanExecute();
            });
        }
        #endregion

        #region IHaveContacts
        public void SetContacts(Response<Model.Api.GetContactsResponse> response, List<ImportContact> addressBook)
        {
            SetBlockedContacts(response, addressBook);
        }

        #endregion
    }
}
