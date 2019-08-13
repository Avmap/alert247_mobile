
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
using Plugin.FirebasePushNotification;
using System;
using System.Threading.Tasks;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
               MainPage = new NavigationPage(new ManageContactsPage());
                //MainPage = new NavigationPage(new MainTabbedPage());
            }
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
            // Handle when your app starts
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var localSettings = ViewModelLocator.Instance.Resolve<ILocalSettingsService>();
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    if (!string.IsNullOrWhiteSpace(p.Token))
                    {
                        localSettings.SaveFirebaseToken(p.Token);
                        //here send registrationid to server
                        var userProfileService = ViewModelLocator.Instance.Resolve<IUserProfileService>();
                        if (userProfileService != null)
                        {

                            Task.Run(async () =>
                            {
                                var userToken = await localSettings.GetAuthToken();
                                await userProfileService.Ping(userToken, 22.1, 22.2, p.Token);
                            });

                        }
                    }

                };
                MessagingCenter.Subscribe<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), (sender, data) =>
                {
                    //here handle messages from firebase on each platform
                });
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
                analyticsService.LogOpenAppEvent();
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
