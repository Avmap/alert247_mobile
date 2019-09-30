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
using Plugin.FirebasePushNotification;
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
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using AlertApp.Resx;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AlertApp
{
    public partial class App : Application
    {
        public static RegistrationField[] TempRegistrationFields { get; set; }
        public App()
        {
            InitializeComponent();
            var isUserRegistered = IsRegister().Result;
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
                MainPage = new NavigationPage(new MainTabbedPage());
            }

#if DEBUG            
            //generatePclCryptoKeys();
#endif
        }

        private async Task<bool> IsRegister()
        {
            string userID = null;
            string token = null;
            try
            {
                userID = await SecureStorage.GetAsync(Settings.UserId);
                token = await SecureStorage.GetAsync(Settings.AuthToken);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                userID = Preferences.Get(Settings.UserId, "");
                token = Preferences.Get(Settings.AuthToken, "");
            }
            if (!string.IsNullOrWhiteSpace(userID))
                return true;

            return false;
        }

        protected override void OnStart()
        {
            RequestPermissions();
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
                                    var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
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
                //   Subscribe();

            }

            var analyticsService = DependencyService.Get<IFirebaseAnalyticsService>();
            if (analyticsService != null)
            {
                analyticsService.LogOpenAppEvent();
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
            // Subscribe();
            RequestPermissions();
        }
        private async void RequestPermissions()
        {
            try
            {
                var locationPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                var contactPermissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);
                if (locationPermissionStatus != PermissionStatus.Granted || contactPermissionStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Location, Permission.Contacts });
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
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

    }
}
