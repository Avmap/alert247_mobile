using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Contacts;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class ManageContactsPageViewModel : BaseViewModel
    {
        #region Services
        readonly IContactsService _contactsService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactProfileImageProvider _contactProfileImageProvider;
        #endregion

        #region Properties
        private ObservableCollection<TabItem> _Tabs;

        public ObservableCollection<TabItem> Tabs
        {
            get { return _Tabs; }
            set
            {
                _Tabs = value;
                OnPropertyChanged("Tabs");
            }
        }

        private bool _ShowCurrentContactMenu;

        public bool ShowCurrentContactMenu
        {
            get { return _ShowCurrentContactMenu; }
            set
            {
                _ShowCurrentContactMenu = value;
                OnPropertyChanged("ShowCurrentContactMenu");
            }
        }


        #endregion

        public ManageContactsPageViewModel(IContactsService contactsService, ILocalSettingsService localSettingsService)
        {
            _contactsService = contactsService;
            _localSettingsService = localSettingsService;
            _contactProfileImageProvider = DependencyService.Get<IContactProfileImageProvider>();

            var tempTabs = new List<TabItem>();

            tempTabs.Add(new TabItem { Name = AppResources.TabMyCommunity, Id = 1, Selected = true }); ;
            tempTabs.Add(new TabItem { Name = AppResources.TabDependands, Id = 2 });
            tempTabs.Add(new TabItem { Name = AppResources.WhoAlertMe, Id = 3 });
            tempTabs.Add(new TabItem { Name = AppResources.BlockedUsersPage, Id = 4 });
            Tabs = new ObservableCollection<TabItem>(tempTabs);
        }

        public async Task<Response<GetContactsResponse>> GetContacts()
        {
            var token = await _localSettingsService.GetAuthToken();
            return await _contactsService.GetContacts(token);
        }

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        public class TabItem : INotifyPropertyChanged
        {

            private string _Name;

            public string Name
            {
                get { return _Name; }
                set
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }


            private bool _Selected;
            public bool Selected
            {
                get { return _Selected; }
                set
                {
                    _Selected = value;
                    OnPropertyChanged("Selected");
                }
            }

            private bool _HasBadge;

            public bool HasBadge
            {
                get { return _HasBadge; }
                set
                {
                    _HasBadge = value;
                    OnPropertyChanged("HasBadge");
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


            public int Id { get; set; }

            #region INotifyPropertyChanged
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            #endregion
        }

    }
}
