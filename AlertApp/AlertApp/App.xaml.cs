using static PCLCrypto.WinRTCrypto;
using ICryptoTransform = System.Security.Cryptography.ICryptoTransform;
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using AlertApp.ViewModels;
using CommonServiceLocator;
using PCLCrypto;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

using AlertApp.Resx;
using AlertApp.Model;
using AlertApp.Views;
using Plugin.FirebasePushNotification;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("materialdesignicons-webfont.ttf", Alias = "Material Design Icons")]
namespace AlertApp
{
    public partial class App : Application
    {
        public static RegistrationField[] TempRegistrationFields { get; set; }
        public App()
        {
            InitializeComponent();
            var isUserRegistered = IsRegister();
            if (!isUserRegistered)
            {
                Preferences.Set(Settings.AlwaysOn, true);
                var languageService = DependencyService.Get<ILocalize>();
                if (languageService != null)
                {
                    var systemlanguage = languageService.GetCurrentCultureInfo();
                    if (systemlanguage != null)
                    {
                        Preferences.Set(Settings.SelectedLanguage, systemlanguage.Name);
                    }
                }
                MainPage = new NavigationPage(new SelectLanguagePage());
            }
            else
            {
                if (Preferences.Get(Settings.DoNotShowWhatsNew, false)) { 
                MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    MainPage = new NavigationPage(new WhatsNewPage(new MainPage()));
                }

            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.Black;
            }
        }

        private async void debugRegister()
        {
            RegistrationFieldsPageViewModel vm;
            vm = ViewModelLocator.Instance.Resolve<RegistrationFieldsPageViewModel>();
            vm.SetBusy(true);
            //var registrationFieldsResponse = await vm.GetRegistrationFieldsAsync();
            //App.TempRegistrationFields = registrationFieldsResponse.Result;
             MainPage = new NavigationPage(new RegistrationFieldsPage(null));
            vm.SetBusy(false);
            
        }

        private bool IsRegister()
        {
            return Preferences.Get(Settings.HasFinishRegistration, false);
        }

        protected override void OnStart()
        {
            RequestPermissions();
            VersionTracking.Track();
            // First time launching app
            var firstLaunchEver = VersionTracking.IsFirstLaunchEver;
            if (firstLaunchEver)
            {
                // Preferences.Set(Settings.HasFinishRegistration, false);
                SetSettings();
            }
            // First time launching current version
            var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;
            if (firstLaunchCurrent)
            {
                var currentVersion = VersionTracking.CurrentVersion;
                if (currentVersion == "0.00.62")
                {
                    SetSettings();
                }
            }
            // Handle when your app starts
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var localSettings = ViewModelLocator.Instance.Resolve<ILocalSettingsService>();
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    if (!string.IsNullOrWhiteSpace(p.Token))
                    {
                        //here send registrationid to server
                        var userProfileService = ViewModelLocator.Instance.Resolve<IUserProfileService>();
                        if (userProfileService != null)
                        {
                            Task.Run(async () =>
                            {
                                Debug.WriteLine("Firebase", "New Token: " + p.Token);
                                localSettings.SaveFirebaseToken(p.Token);
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

                                var authToken = await localSettings.GetAuthToken();
                                if (!string.IsNullOrWhiteSpace(authToken))
                                {
                                    await userProfileService.Ping(authToken, location != null ? location.Latitude : (double?)null, location != null ? location.Longitude : (double?)null, p.Token);
                                }
                            });

                        }
                    }

                };

                if (Device.RuntimePlatform == Device.iOS)
                {
                    CrossFirebasePushNotification.Current.OnNotificationReceived += async (s, p) =>
                    {
                        await HandleNotifications(p.Data);
                    };

                    CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
                    {
                        await HandleNotifications(p.Data);
                    };
                }

                Subscribe();
            }

            var analyticsService = DependencyService.Get<IFirebaseAnalyticsService>();
            if (analyticsService != null)
            {
                analyticsService.LogOpenAppEvent();
            }
        }

        private async Task HandleNotifications(IDictionary<string, object> data)
        {
            data.TryGetValue("messageType", out var messageType);

            if (((string)messageType) == "contact")
            {
                data.TryGetValue("cellphone", out var cellphone);

                var contact = new Model.Api.Contact { Cellphone = (string)cellphone, NotificationId = 0 };
                await Current.MainPage.Navigation.PushModalAsync(new CommunityRequestPage(contact));
            }
            else if (((string)messageType) == "alert")
            {
                data.TryGetValue("alertType", out var alertType);
                data.TryGetValue("position", out var position);
                data.TryGetValue("profiledata", out var profiledata);
                data.TryGetValue("filekey", out var filekey);
                data.TryGetValue("cellphone", out var cellphone);
                data.TryGetValue("pkey", out var pkey);
                data.TryGetValue("alertTime", out var alertTime);
                data.TryGetValue("alertID", out var alertId);

                var notificationData = new NotificationAction();
                notificationData.Type = (int)alertType;
                notificationData.NotificationId = 0;
                notificationData.Data = new AlertNotificationData
                {
                    FileKey = (string)filekey,
                    ProfileData = (string)profiledata,
                    Position = (string)position,
                    AlertType = (int)alertType,
                    Cellphone = (string)cellphone,
                    AlertId = (int?)alertId,
                    AlertTime = (string)alertTime,
                    PublicKey = (string)pkey
                };
                await Current.MainPage.Navigation.PushModalAsync(new AlertRespondPage(notificationData));
            }
        }

        protected override void OnSleep()
        {
            var analyticsService = DependencyService.Get<IFirebaseAnalyticsService>();
            if (analyticsService != null)
            {
                analyticsService.LogOpenSleepEvent();
            }
            MessagingCenter.Unsubscribe<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString());
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Subscribe();
            RequestPermissions();
        }

        private async void RequestPermission(string PermissionName)
        {
            PermissionStatus ps;
            try
            {
                switch(PermissionName)
                {
                    case "Location":
                        ps = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                        if (ps != PermissionStatus.Granted)
                        {
                            var results = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                        }
                        break;
                    case "Contacts":
                        ps = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                        if (ps != PermissionStatus.Granted)
                        {
                            var results2 = await Permissions.RequestAsync<Permissions.ContactsRead>();
                        }
                        break;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, PermissionName+" - Feature not supported: " + fnsEx.Message, "OK");

                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, PermissionName + " - Feature not enabled: " + fneEx.Message, "OK");
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, PermissionName + " - Permission error: " + pEx.Message, "OK");
                // Handle permission exception
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.Error, PermissionName + " - Other error: " + ex.Message, "OK");
            }
        }

        private void RequestPermissions()
        {
            RequestPermission("Location");
            RequestPermission("Contacts");
           
            var locationSettingsService = DependencyService.Get<ILocationSettings>();
            var notificationService = DependencyService.Get<INotificationManager>();
            if (!locationSettingsService.IsLocationEnabled())
            {
                notificationService.ToastNotification(AppResources.LocationServicesOffMessage);
            }
        }


        private void Subscribe()
        {
            MessagingCenter.Subscribe<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), (sender, data) =>
            {
                //here handle messages from firebase on each platform
                if (data is NotificationAction)
                {
                    Debug.WriteLine("Hanlde once", "New asdaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa:");
                    //Current.MainPage.Navigation.PushAsync(new AlertRespondPage(data as NotificationAction), true);
                }
            });

        }

        private void SetSettings()
        {
            Preferences.Set(Settings.SOS_BUTTON_VISIBLE, true);
            Preferences.Set(Settings.THREAT_BUTTON_VISIBLE, true);
            Preferences.Set(Settings.FIRE_BUTTON_VISIBLE, true);
            Preferences.Set(Settings.ACCIDENT_BUTTON_VISIBLE, true);
            Preferences.Set(Settings.CONTACTS_BUTTON_VISIBLE, true);
            Preferences.Set(Settings.INFORMATION_BUTTON_VISIBLE, true);
        }

    }
}
