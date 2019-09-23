using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Pages;
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
    public class WhoAlertsMePageViewModel : BaseViewModel, IHaveContacts
    {
        #region Properties
        private ObservableCollection<Contact> _AlertMeContacts;
        public ObservableCollection<Contact> AlertMeContacts
        {
            get
            {
                if (_AlertMeContacts == null)
                {
                    _AlertMeContacts = new ObservableCollection<Contact>();
                }
                return _AlertMeContacts;
            }
            set
            {
                _AlertMeContacts = value;
                OnPropertyChanged("AlertMeContacts");
                OnPropertyChanged("HasContacts");
            }
        }

        public bool HasContacts => AlertMeContacts == null || AlertMeContacts.Count == 0;

        #endregion

        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        #region Commands

        private ICommand _GetAlertMeCommand;
        public ICommand GetAlertMeCommand
        {
            get
            {
                return _GetAlertMeCommand ?? (_GetAlertMeCommand = new Command(GetAlertMe, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        public WhoAlertsMePageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();
            SetBusy(true);
        }

        private void GetAlertMe()
        {
            SetBusy(true);
            MessagingCenter.Send((BaseViewModel)this, RefreshContactsEvent.Event, new RefreshContactsEvent { });
        }

        public async void NavigateToAcceptCommunityReqeustScreen(Contact contact)
        {
            SetBusy(true);
            if (contact.ProfileImageUri != null)
            {
                var image = _contactProfileImageProvider.GetProfileImage(contact.ProfileImageUri);
                contact.ProfileImage = image;
            }

            var modal = new CommunityRequestPage(contact);
            modal.Disappearing += (sender2, e2) =>
            {
                SetBusy(false);
                var vm = modal.BindingContext as CommunityRequestPageViewModel;
                if (vm.HasChange)
                {
                    var parentPage = NavigationService.NavigationStack.LastOrDefault() as ManageContactsPage;
                    if (parentPage != null)
                        parentPage.RefreshContacts();

                }
            };
            await NavigationService.PushModalAsync(modal);
        }

        private void Modal_Disappearing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void SetAlertMeContacts(Response<GetContactsResponse> webServiceContacts)
        {
            if (webServiceContacts != null && webServiceContacts.IsOk)
            {
                var community = webServiceContacts.Result.Contacts.AlertMe;
                if (community != null && community.Count > 0)
                {
                    var addressBook = await GetAddressbook();
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        AlertMeContacts.Clear();
                        foreach (var item in community)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();
                            if (addressBookItem != null)
                            {
                                AlertMeContacts.Add(new Contact { ProfileImageUri = addressBookItem.PhotoUri, Accepted = item.Accepted, Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                AlertMeContacts.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }
                        SetBusy(false);
                    }
                    else
                    {
                        var contacts = community.Select(c => new Contact { Accepted = c.Accepted, Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        AlertMeContacts.Clear();
                        foreach (var item in contacts)
                        {
                            AlertMeContacts.Add(item);
                        }
                        SetBusy(false);
                    }
                }
                else
                {
                    SetBusy(false);
                }

            }
            else
            {
                SetBusy(false);
            }
            OnPropertyChanged("HasContacts");

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
                ((Command)GetAlertMeCommand).ChangeCanExecute();
            });
        }
        #endregion

        #region IHaveContacts
        public void SetContacts(Response<GetContactsResponse> response)
        {
            SetAlertMeContacts(response);
        }

        #endregion
    }
}
