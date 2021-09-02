using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using AlertApp.Services.Contacts;
using AlertApp.Services.News;
using AlertApp.Services.Subscription;
using AlertApp.Utils;
using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace AlertApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Services
        readonly IUserProfileService _userProfileService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IContactsService _contactsService;
        readonly INewsService _newsService;
        readonly ISubscriptionService _subscriptionService;
        #endregion

        #region Commands

        private ICommand _OpenSettingsCommand;
        public new ICommand OpenSettingsCommand
        {
            get
            {
                return _OpenSettingsCommand ?? (_OpenSettingsCommand = new Command(OpenSettingsScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _OpenAddContactsCommand;
        public new ICommand OpenAddContactsCommand
        {
            get
            {
                return _OpenAddContactsCommand ?? (_OpenAddContactsCommand = new Command(AddContactsScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _OpenAddSubCommand;
        public new ICommand OpenAddSubCommand
        {
            get
            {
                return _OpenAddSubCommand ?? (_OpenAddSubCommand = new Command(AddSubLink, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _OpenTouristGuide;

        public new ICommand OpenTouristGuide
        {
            get
            {
                return _OpenTouristGuide ?? (_OpenTouristGuide = new Command(TouristGuideScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _SosCommand;
        public ICommand SosCommand
        {
            get
            {
                return _SosCommand ?? (_SosCommand = new Command<string>(OpenSendAlertScreen, (alertType) =>
                {
                    return !Busy;
                }));
            }
        }


        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new Command(CancelSendAlert, () =>
                {
                    return true;
                }));
            }
        }

        private ICommand _OpenContactsScreen;
        public ICommand OpenContactsScreen
        {
            get
            {
                return _OpenContactsScreen ?? (_OpenContactsScreen = new Command(NavigateToContactScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _RefreshNewsCommand;
        public ICommand RefreshNewsCommand
        {
            get
            {
                return _RefreshNewsCommand ?? (_RefreshNewsCommand = new Command(RefreshNews, () =>
                {
                    return !Busy;
                }));
            }
        }

        #endregion

        #region Properties

        private string _SosButtonText;
        public string SosButtonText
        {
            get { return _SosButtonText; }
            set
            {
                _SosButtonText = value;
                OnPropertyChanged("SosButtonText");
            }
        }


        private string _CancelButtonText;
        public string CancelButtonText
        {
            get { return _CancelButtonText; }
            set
            {
                _CancelButtonText = value;
                OnPropertyChanged("CancelButtonText");
            }
        }


        public bool ShowSosButton
        {
            get { return !ShowCancelButton && Preferences.Get(Settings.SOS_BUTTON_VISIBLE, true); }
        }

        private bool _IsRefreshingNews;

        public bool IsRefreshingNews
        {
            get { return _IsRefreshingNews; }
            set
            {
                _IsRefreshingNews = value;
                OnPropertyChanged("IsRefreshingNews");
            }
        }

        private ObservableCollection<NewsEntry> _MyNews;
        public ObservableCollection<NewsEntry> MyNews
        {
            get
            {
                if (_MyNews == null)
                {
                    _MyNews = new ObservableCollection<NewsEntry>();
                }
                return _MyNews;
            }
            set
            {
                _MyNews = value;
                OnPropertyChanged("MyNews");
            }
        }

        private int _NumberOfContacts;

        public int NumberOfContacts
        {
            get
            {
                return _NumberOfContacts;
            }
            set
            {
                _NumberOfContacts = value;
                OnPropertyChanged("NumberOfContacts");
                OnPropertyChanged("HasContacts");
                OnPropertyChanged("DoesNotHaveContacts");
                OnPropertyChanged("SingleContact");
                OnPropertyChanged("MultipleContacts");
                OnPropertyChanged("CanSendAlert");
                OnPropertyChanged("CanNotSendAlert");
                OnPropertyChanged("ShowContactsButton");
            }
        }

        public bool HasContacts { get { return _NumberOfContacts > 0; } }
        public bool DoesNotHaveContacts { get { 
                return (Preferences.Get(Settings.CONTACTS_BUTTON_VISIBLE, true) && (_NumberOfContacts<1)); 
            } 
        }

        public bool SingleContact { get { return _NumberOfContacts==1; } }
        public bool MultipleContacts { get { return _NumberOfContacts >1; } }

        public bool HasSub { get { return (isSubOK||IsSubExpiring); } }   

        public bool DoesNotHaveSub { get { return !HasSub; } }

        public bool CanSendAlert { get { return (HasSub || HasContacts); } }

        public bool CanNotSendAlert { get { return !CanSendAlert; } }

        private bool _ShowCancelButton;
        public bool ShowCancelButton
        {
            get { return _ShowCancelButton; }
            set
            {
                _ShowCancelButton = value;
                OnPropertyChanged("ShowCancelButton");
                OnPropertyChanged("ShowSosButton");
                OnPropertyChanged("ColorPressToCancel");
                OnPropertyChanged("showNonCritical");
            }
        }

        public bool showNonCritical
        {
            get { return !ShowCancelButton; }
        }

        bool stop = false;
        int maxSeconds = 5;
        int currentSecond = 0;
        Model.AlertType alertType;

        public Color ColorPressToCancel => ShowCancelButton ? Color.Black : Color.Transparent;

        public bool ShowThreatButton => Preferences.Get(Settings.THREAT_BUTTON_VISIBLE, true);
        public bool ShowFireButton => Preferences.Get(Settings.FIRE_BUTTON_VISIBLE, true);
        public bool ShowAccidentButton => Preferences.Get(Settings.ACCIDENT_BUTTON_VISIBLE, true);

        public bool ShowContactsButton
        {
            get
            {
                return HasContacts && Preferences.Get(Settings.CONTACTS_BUTTON_VISIBLE, true);
            }
        }

        public bool ShowSubscriptionButton
        {
            get
            {
                return Preferences.Get(Settings.SUBSCRIPTION_BUTTON_VISIBLE, true) && DoesNotHaveSub;
            }
        }

        public bool ShowMapButton => Preferences.Get(Settings.MAP_BUTTON_VISIBLE, true);

        private string _SubscriptionStart;
        public string SubscriptionStart
        {
            get
            {
                return _SubscriptionStart;
            }
            set
            {
                _SubscriptionStart = value;
                OnPropertyChanged("SubscriptionStart");
            }
        }

        private string _SubscriptionEnd;
        public string SubscriptionEnd
        {
            get
            {
                return _SubscriptionEnd;
            }
            set
            {
                _SubscriptionEnd = value;
                OnPropertyChanged("SubscriptionEnd");
            }
        }

        private string _SubscriptionPackage;
        public string SubscriptionPackage
        {
            get
            {
                return _SubscriptionPackage;
            }
            set
            {
                _SubscriptionPackage = value;
                OnPropertyChanged("SubscriptionPackage");
            }
        }


        private bool _isSubOK;
        public bool isSubOK
        {
            get
            {
                return _isSubOK;
            }
            set
            {
                _isSubOK = value;
                OnPropertyChanged("isSubOK");
                OnPropertyChanged("IsSubExpiring");
                OnPropertyChanged("IsSubExpired");
                OnPropertyChanged("IsSubInactive");
                OnPropertyChanged("HasSub");
                OnPropertyChanged("DoesNotHaveSub");
            }
        }

        private bool _IsSubExpiring;
        public bool IsSubExpiring
        {
            get
            {
                return _IsSubExpiring;
            }
            set
            {
                _IsSubExpiring = value;
                OnPropertyChanged("isSubOK");
                OnPropertyChanged("IsSubExpiring");
                OnPropertyChanged("IsSubExpired");
                OnPropertyChanged("IsSubInactive");
                OnPropertyChanged("HasSub");
                OnPropertyChanged("DoesNotHaveSub");
            }
        }

        private bool _IsSubExpired;
        public bool IsSubExpired
        {
            get
            {
                return _IsSubExpired;
            }
            set
            {
                _IsSubExpired = value;
                OnPropertyChanged("isSubOK");
                OnPropertyChanged("IsSubExpiring");
                OnPropertyChanged("IsSubExpired");
                OnPropertyChanged("IsSubInactive");
                OnPropertyChanged("HasSub");
                OnPropertyChanged("DoesNotHaveSub");
            }
        }

        private bool _IsSubInactive;
        public bool IsSubInactive
        {
            get
            {
                return _IsSubInactive;
            }
            set
            {
                _IsSubInactive = value;
                OnPropertyChanged("isSubOK");
                OnPropertyChanged("IsSubExpiring");
                OnPropertyChanged("IsSubExpired");
                OnPropertyChanged("IsSubInactive");
                OnPropertyChanged("HasSub");
                OnPropertyChanged("DoesNotHaveSub");
            }
        }
        #endregion

        public MainPageViewModel(
            IUserProfileService userProfileService,
            ILocalSettingsService localSettingsService,
            IContactsService contactsService,
            INewsService newsService,
            ISubscriptionService subscriptionService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
            _contactsService = contactsService;
            _newsService = newsService;
            _subscriptionService = subscriptionService;

            SosButtonText = "SOS";

            var appHasRun = _localSettingsService.GetAppHasRunSetting();
            if (!appHasRun)
            {
                var guardian = DependencyService.Get<IGuardian>();
                if (guardian != null)
                {
                    guardian.StartGuardianService();
                }
                _localSettingsService.SaveAppHasRunSetting(true);
            }
            PingServer();
            RefreshSubInfo();
        }

        public async void RefreshSubInfo()
        {
            var r = await this.GetSubInfo();
            
        }

        public async Task<Subscription> GetSubInfo()
        {
            
            var token = await _localSettingsService.GetAuthToken();
            var S = new Subscription();
            DateTime? startDate=null;
            DateTime? endDate = null;
            try
            {
                Response<SubscriptionResponse> r2 = await _subscriptionService.GetSubscriptionInfo(token); 
                S = r2.Result.Subscription;
                this.SubscriptionStart = S.Start;
                this.SubscriptionEnd = S.End;
                this.SubscriptionPackage = S.Package;
                try
                {
                    startDate = DateTime.ParseExact(S.Start, "yyyy-MM-dd HH:mm:ss",null);
                } catch{}

                try
                {
                    endDate = DateTime.ParseExact(S.End, "yyyy-MM-dd HH:mm:ss", null);
                }
                catch { }

                if (!startDate.HasValue || !endDate.HasValue)
                {
                    this.IsSubInactive = true;
                }
                else
                {
                    if (startDate > DateTime.Now){
                        this.IsSubInactive = true;
                    }
                    else
                    {
                        if (endDate  < DateTime.Now)
                        {
                            this.IsSubExpired = true;
                        }
                        else
                        {
                            if (((DateTime)endDate - DateTime.Now).TotalDays <= 7)
                            {
                                this.IsSubExpiring = true;
                            }
                            else
                            {
                                this.isSubOK = true;
                            }
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
                this.IsSubInactive = true;
#endif
            }
            return S;

            
        }

        public async void RefreshNews()
        {
            await Task.Run(async () =>
            {
                var r = await this.GetNews();
                ObservableCollection<AlertApp.Model.Api.NewsEntry> collection = new ObservableCollection<AlertApp.Model.Api.NewsEntry>(r);
                var s = await this.GetSubInfo();
                var subscriptionItem = new AlertApp.Model.Api.NewsEntry();
                var translate = new TranslateExtension();
                
                subscriptionItem.PublishDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                if (isSubOK)
                {
                    subscriptionItem.Category = NewsEntryCategory.SUCCESS;
                    subscriptionItem.Title = String.Format("{0}: {1}", translate.GetTranslatedValue("SubscriptionFrame"),translate.GetTranslatedValue("SubscriptionStatusOK"));
                    subscriptionItem.Description = String.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                    translate.GetTranslatedValue("SubscriptionStart"),
                    s.Start,
                    translate.GetTranslatedValue("SubscriptionEnd"),
                    s.End,
                    translate.GetTranslatedValue("SubscriptionPackage"),
                    s.Package);
                }
                else if (IsSubExpiring) {
                    subscriptionItem.Category = NewsEntryCategory.WARNING;
                    subscriptionItem.Title = String.Format("{0}: {1}", translate.GetTranslatedValue("SubscriptionFrame"), translate.GetTranslatedValue("SubscriptionStatusExpiring"));
                    subscriptionItem.Description = String.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                    translate.GetTranslatedValue("SubscriptionStart"),
                    s.Start,
                    translate.GetTranslatedValue("SubscriptionEnd"),
                    s.End,
                    translate.GetTranslatedValue("SubscriptionPackage"),
                    s.Package);
                }
                else if (IsSubExpired) {
                    subscriptionItem.Category = NewsEntryCategory.DANGER;
                    subscriptionItem.Title = String.Format("{0}: {1}", translate.GetTranslatedValue("SubscriptionFrame"), translate.GetTranslatedValue("SubscriptionStatusExpired"));
                    subscriptionItem.Description = String.Format("{0}: {1}, {2}: {3}, {4}: {5}",
                    translate.GetTranslatedValue("SubscriptionStart"),
                    s.Start,
                    translate.GetTranslatedValue("SubscriptionEnd"),
                    s.End,
                    translate.GetTranslatedValue("SubscriptionPackage"),
                    s.Package);
                }
                else if (IsSubInactive) {
                    subscriptionItem.Category = NewsEntryCategory.DANGER;
                    subscriptionItem.Title = String.Format("{0}: {1}", translate.GetTranslatedValue("SubscriptionFrame"), translate.GetTranslatedValue("SubscriptionStatusInactive"));
                }
                
                collection.Insert(0, subscriptionItem);
                this.MyNews = collection;

                this.IsRefreshingNews = false;
            });
        }

        public async Task<int> GetContacts()
        {
            var token = await _localSettingsService.GetAuthToken();

            int cnt = 0;
            try
            {
                Response<GetContactsResponse> contacts = await _contactsService.GetContacts(token);
                cnt = contacts.Result.Contacts.Community.Count;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
            return cnt;

        }

        public async Task<List<NewsEntry>> GetNews()
        {
            var token = await _localSettingsService.GetAuthToken();
            var r = new List<NewsEntry>();
            try
            {
#if DEBUG
                Response<NewsEntryResponse> r2 = await _newsService.GetNewsMock(token); 
#else
Response<NewsEntryResponse> r2 = await _newsService.GetNews(token); 
#endif
                r = r2.Result.News;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
            }
            return r;

        }

        private async void PingServer()
        {
            var userToken = await _localSettingsService.GetAuthToken();
            var firebaseToken = Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.Token;
            Location location = null;
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (locationPermissionStatus == PermissionStatus.Granted)
                {
                    location = await Geolocation.GetLastKnownLocationAsync();
                }
            }
            catch (Exception ex)
            {

            }
            await _userProfileService.Ping(userToken, location != null ? location.Latitude : (double?)null, location != null ? location.Longitude : (double?)null, firebaseToken);

            //var locales = await TextToSpeech.GetLocalesAsync();

            //// Grab the first locale
            //var locale = locales.LastOrDefault();
            //var settings = new SpeechOptions()
            //{
            //    Volume = 1f,
            //    Pitch = 1.0f,
            //    Locale = locale
            //};
            //await TextToSpeech.SpeakAsync("Hello from Barcelona", settings);

        }

        private async void NavigateToContactScreen()
        {
            SetBusy(true);
            await Application.Current.MainPage.Navigation.PushAsync(new ManageContactsPage(), false);
            SetBusy(false);
        }

        private async void OpenSendAlertScreen(string alertType)
        {
            switch (alertType)
            {
                case "manual":
                    this.alertType = Model.AlertType.UserAlert;
                    break;
                case "fire":
                    this.alertType = Model.AlertType.Fire;
                    break;
                case "police":
                    this.alertType = Model.AlertType.Police;
                    break;
                case "health":
                    this.alertType = Model.AlertType.Health;
                    break;
            }
            SetBusy(true);
            ShowCancelButton = true;
            stop = false;
            StartTimer();
        }
        private async void CancelSendAlert()
        {
            currentSecond = 0;
            Device.BeginInvokeOnMainThread(() =>
            {
                CancelButtonText = maxSeconds.ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
            });
            SetBusy(false);
            stop = true;
            ShowCancelButton = false;
        }

        private async void OpenSettingsScreen()
        {
            SetBusy(true);
            await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage(), false);
            SetBusy(false);
        }

        private async void AddContactsScreen()
        {
            SetBusy(true);
            await Application.Current.MainPage.Navigation.PushAsync(new AddContactPage(new List<string>()), false);
            //await
            //ice.PushAsync(new AddContactPage(new List<string>()), false);
            SetBusy(false);
        }

        private async void AddSubLink()
        {
            SetBusy(true);
            //await Launcher.OpenAsync(new Uri(AlertApp.CodeSettings.SubscriptionURL));
            await Application.Current.MainPage.Navigation.PushAsync(new SubscriptionPage(), false);
            SetBusy(false);
        }

        private async void TouristGuideScreen()
        {
            SetBusy(true);
            await Application.Current.MainPage.Navigation.PushAsync(new MapPage(), false);
            SetBusy(false);
        }

        private async void StartTimer()
        {
            for (int i = currentSecond; i < maxSeconds; i++)
            {
                if (stop)
                {
                    ShowCancelButton = false;
                    i = maxSeconds;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CancelButtonText = maxSeconds.ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
                    });
                    break;
                }

                if (!stop)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CancelButtonText = (maxSeconds - i).ToString();//AppResources.CancelSendAlert + "\n" + (seconds - i);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

            }

            if (!stop)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SendingAlertPage(alertType), false);
                SetBusy(false);
            }

            ShowCancelButton = false;
        }

        public void HideShowButtons()
        {
            OnPropertyChanged("ShowSosButton");
            OnPropertyChanged("ShowThreatButton");
            OnPropertyChanged("ShowFireButton");
            OnPropertyChanged("ShowAccidentButton");
            OnPropertyChanged("ShowContactsMenuButton");
            OnPropertyChanged("ShowInfoMenuButton");
            OnPropertyChanged("HasContacts");
            OnPropertyChanged("ShowContactsButton");
            OnPropertyChanged("HasSub");
            OnPropertyChanged("ShowSubscriptionButton");
            OnPropertyChanged("ShowMapButton");
        }

#region BaseViewModel

        public override void SetBusy(bool isBusy)
        {

            this.Busy = isBusy;
            ((Command)OpenContactsScreen).ChangeCanExecute();
            ((Command)SosCommand).ChangeCanExecute();

        }

#endregion
    }
}

