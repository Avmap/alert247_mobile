﻿
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using Plugin.FirebasePushNotification;
using System;
using System.Threading.Tasks;
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
                //MainPage = new NavigationPage(new RegistrationFieldsPage(new RegistrationField []{ }));
                MainPage = new NavigationPage(new MainPage());
            }

        }

        private async Task<bool> IsRegister()
        {
            string userID = null;
            try
            {
                userID =  await SecureStorage.GetAsync(Settings.UserId);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                userID =  Preferences.Get(Settings.UserId, "");
            }
            if (!string.IsNullOrWhiteSpace(userID))
                return true;

            return false;
        }

        private async void TEST()
        {
            var crypto = new CryptographyService(new LocalSettingsService());
            crypto.GenerateKeys("1770");
            
            //var entrypted = crypto.Encrypt("thanos", "1770");
            //var decrypted = crypto.Decrypt("KhcyLh+CWe1d3PID2zRYvQ==;ADkE8o2ftXL0Q4E1lWMBMgd==", "1770");

            var profileData = await crypto.EncryptProfileData("i am thanos");

            var decryptProfileData = await crypto.DecryptProfileData(profileData);

        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    //here send registrationid to server
                };
                MessagingCenter.Subscribe<ICrossFirebase, object>(this, typeof(ICrossFirebase).ToString(), (sender, data) =>
                {
                    //here handle messages from firebase on each platform
                });
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
