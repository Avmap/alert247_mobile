
using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Utils;
using Plugin.FirebasePushNotification;
using System;
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
            var isUserRegistered = false;// Xamarin.Essentials.Preferences.Get(Settings.IsRegistered,false);
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
               // MainPage = new NavigationPage(new EnterApplicationPinCodePage());               
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
           
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
