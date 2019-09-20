using AlertApp.Model;
using AlertApp.Services;
using AlertApp.Services.Alert;
using AlertApp.Services.Community;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using AlertApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.Utils
{
    public class ViewModelProvider
    {
        #region ViewModels
        internal static SelectLanguagePageModel SelectLanguageViewModel()
        {
            return new SelectLanguagePageModel(provideLocalSettingsService());
        }

        internal static EnterMobileNumberPageModel EnterMobileNumberViewModel()
        {
            return new EnterMobileNumberPageModel();
        }

        internal static EnterActivationCodePageModel EnterActivationCodeViewModel(string mobilenumber)
        {
            return new EnterActivationCodePageModel(provideRegistrationService(), provideLocalSettingsService(), mobilenumber);
        }

        internal static RegistrationFieldsPageViewModel RegistrationFieldsPageViewModel()
        {
            return new RegistrationFieldsPageViewModel(provideUserProfileService(), provideLocalSettingsService());
        }

 
        internal static MainPageViewModel MainPageViewModel()
        {
            return new MainPageViewModel(provideUserProfileService(),provideLocalSettingsService());
        }

        internal static DialogSelectLanguageViewModel DialogSelectLanguageViewModel()
        {
            return new DialogSelectLanguageViewModel(provideLocalSettingsService());
        }

      
        internal static DependandsPageViewModel DependandsPageViewModel()
        {
            return new DependandsPageViewModel(provideCommunityService(), provideLocalSettingsService());
        }
        #endregion

        #region Services

        private static IUserProfileService provideUserProfileService()
        {
            return new UserProfileService(provideLocalSettingsService(),provideCryptographyService());
        }
        private static ILocalSettingsService provideLocalSettingsService()
        {
            return new LocalSettingsService();
        }
        private static ICryptographyService provideCryptographyService()
        {   
            //var cryptoService = DependencyService.Get<ICryptographyService>();
            //return cryptoService;
            return new CryptographyService(provideLocalSettingsService());
        }

        private static ICommunityService provideCommunityService()
        {
            return new CommunityService();
        }
        private static IRegistrationService provideRegistrationService()
        {
            return new RegistrationService();
        }
        
        #endregion
    }
}
