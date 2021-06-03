using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
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

        private int _NotificationCount;

        public int NotificationCount
        {
            get { return _NotificationCount; }
            set
            {
                _NotificationCount = value;
                OnPropertyChanged("NotificationCount");
            }
        }

        private Color _NotificationBackColor;

        public Color NotificationBackColor
        {
            get { return _NotificationBackColor; }
            set
            {
                _NotificationBackColor = value;
                OnPropertyChanged("NotificationBackColor");
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
            NotificationBackColor = Color.White;
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

            var modal = new CommunityRequestPage(contact);
            modal.Disappearing += (sender2, e2) =>
            {
                SetBusy(false);
                var vm = modal.BindingContext as CommunityRequestPageViewModel;
                if (vm.HasChange)
                {
                    var parentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() as ManageContactsPage;
                    if (parentPage != null)
                    {
                        SetBusy(true);
                        parentPage.RefreshContacts();
                    }
                }
            };
            await Application.Current.MainPage.Navigation.PushModalAsync(modal);
        }

        private void Modal_Disappearing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void SetAlertMeContacts(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            if (response != null && response.IsOk)
            {
                var community = response.Result.Contacts.AlertMe;
                if (community != null && community.Count > 0)
                {
                    NotificationBackColor = Color.FromHex("#800000");
                    NotificationCount = response.Result.Contacts.AlertMe.Where(c => c.Accepted == false).Count();
                    if (NotificationCount == 0)
                    {
                        RemoveBadgeNotification();
                    }
                    //search in addressBook for contacts
                    if (addressBook != null)
                    {
                        var tempContacts = new List<Contact>();
                        foreach (var item in community)
                        {
                            var addressBookItem = addressBook.Where(c => c.FormattedNumber == item.Cellphone).FirstOrDefault();

                            if (addressBookItem != null)
                            {
                                tempContacts.Add(new Contact { ProfileImageUri = addressBookItem.PhotoUri, Accepted = item.Accepted, Cellphone = item.Cellphone, FirstName = addressBookItem.Name, Stats = item.Stats, ProfileImage = addressBookItem.ProfileImage });
                            }
                            else
                            {
                                tempContacts.Add(new Contact { Accepted = item.Accepted, Cellphone = item.Cellphone, Stats = item.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") });
                            }
                        }

                        AlertMeContacts = new ObservableCollection<Contact>(tempContacts);
                        SetBusy(false);
                    }
                    else
                    {
                        var contacts = community.Select(c => new Contact { Accepted = c.Accepted, Cellphone = c.Cellphone, Stats = c.Stats, ProfileImage = ImageSource.FromFile("account_circle.png") }).ToList();
                        var tempContacts = new List<Contact>();
                        foreach (var item in contacts)
                        {
                            tempContacts.Add(item);
                        }
                        AlertMeContacts = new ObservableCollection<Contact>(tempContacts);
                        SetBusy(false);
                    }
                }
                else
                {
                    RemoveBadgeNotification();
                    AlertMeContacts = new ObservableCollection<Contact>();
                    SetBusy(false);
                }

            }
            else
            {
                RemoveBadgeNotification();
                SetBusy(false);
            }
            OnPropertyChanged("HasContacts");

        }

        private void RemoveBadgeNotification()
        {
            NotificationCount = 0;
            NotificationBackColor = Color.White;
        }


        public async Task<bool> RemoveUser(Contact contact)
        {
            SetBusy(true);
            List<string> contacts = new List<string>();
            contacts.Add(contact.Cellphone);
            var response = await _contactsService.RemoveNotifiers(await _localSettingsService.GetAuthToken(), contacts);
            if (response.IsOk)
            {
                GetAlertMe();
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
            var response = await _contactsService.BlockNotifier(await _localSettingsService.GetAuthToken(), contact.Cellphone);
            if (response.IsOk)
            {
                GetAlertMe();
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
                ((Command)GetAlertMeCommand).ChangeCanExecute();
            });
        }
        #endregion

        #region IHaveContacts
        public void SetContacts(Response<GetContactsResponse> response, List<ImportContact> addressBook)
        {
            SetAlertMeContacts(response, addressBook);
        }

        #endregion
    }
}
