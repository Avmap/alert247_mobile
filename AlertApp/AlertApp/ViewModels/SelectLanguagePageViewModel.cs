using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SelectLanguagePageViewModel : BaseViewModel
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties

        private int _LanguageSelectedIndex;

        public int LanguageSelectedIndex
        {
            get { return _LanguageSelectedIndex; }
            set
            {
                _LanguageSelectedIndex = value;
                OnPropertyChanged("LanguageSelectedIndex");
                OnPropertyChanged("SelectedLanguageFlag");
                OnPropertyChanged("SelectedLanguageName");
            }
        }
        private string _SelectedLanguageFlag;

        public string SelectedLanguageFlag
        {
            get { return Language.SupportedLanguages[LanguageSelectedIndex].Flag; }
            set
            {
                _SelectedLanguageFlag = value;
                OnPropertyChanged("SelectedLanguageFlag");
            }
        }
        private string _SelectedLanguageName;

        public string SelectedLanguageName
        {
            get { return Language.SupportedLanguages[LanguageSelectedIndex].Name; }
            set
            {
                _SelectedLanguageName = value;
                OnPropertyChanged("SelectedLanguageName");
            }
        }

        public List<Language> Languages => Language.SupportedLanguages;

        public string Version
        {
            get
            {
                var ts = new AlertApp.Infrastructure.TranslateExtension();
                return String.Format("{0} {1}", ts.GetTranslatedValue("Version"), VersionTracking.CurrentVersion);
            }
        }
        

        #endregion

        #region Commands
        private ICommand _ContinueCommand;
        public ICommand ContinueCommand
        {
            get
            {
                return _ContinueCommand ?? (_ContinueCommand = new Command(Continue, () =>
                {
                    return !Busy;
                }));
            }
        }
        #endregion

        #region Commands
        private ICommand _SelectLanguageCommand;
        public ICommand SelectLanguageCommand
        {
            get
            {
                return _SelectLanguageCommand ?? (_SelectLanguageCommand = new Command<string>(SetSelectedLanguage, (lang) =>
                {
                    return true;
                }));
            }
        }

        private ICommand _SelectLanguageCommandAndBack;
        public ICommand SelectLanguageCommandAndBack
        {
            get
            {
                return _SelectLanguageCommandAndBack ?? (_SelectLanguageCommandAndBack = new Command<string>(SetSelectedLanguageAndBack, (lang) =>
                {
                    return true;
                }));
            }
        }
        #endregion


        public SelectLanguagePageViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;         
        }

        private async void SetSelectedLanguage(string language)
        {
            _localSettingsService.SaveSelectedLanguage(language);

            await RequestPermissions();

            CultureInfo ci = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Resx.AppResources.Culture = ci;
            await Application.Current.MainPage.Navigation.PushAsync(new EnterMobileNumberPage(), false);
        }

        private async void SetSelectedLanguageAndBack(string language)
        {
            _localSettingsService.SaveSelectedLanguage(language);

            await RequestPermissions();

            CultureInfo ci = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Resx.AppResources.Culture = ci;
            App.Current.MainPage = new NavigationPage(new MainPage());
            await App.Current.MainPage.Navigation.PushAsync(new SettingsPage(), false);       
        }

        private void SetSelectedLanguage()
        {
            var selectedLanguage = Preferences.Get(Settings.SelectedLanguage, "");
            if (!string.IsNullOrWhiteSpace(selectedLanguage))
            {
                LanguageSelectedIndex = Language.SupportedLanguages.FindIndex(l => l.NetLanguageName.Equals(selectedLanguage));
            }
            else
            {
                var languageService = DependencyService.Get<ILocalize>();
                if (languageService != null)
                {
                    var systemlanguage = languageService.GetCurrentCultureInfo();
                    if (systemlanguage != null)
                    {
                        _localSettingsService.SaveSelectedLanguage(systemlanguage.Name);
                    }
                }
            }
        }

        public async void Continue()
        {
            await RequestPermissions();
            _localSettingsService.SaveSelectedLanguage(Language.SupportedLanguages[LanguageSelectedIndex].NetLanguageName);
            CultureInfo ci = new CultureInfo(Language.SupportedLanguages[LanguageSelectedIndex].NetLanguageName);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Resx.AppResources.Culture = ci;
            await Application.Current.MainPage.Navigation.PushAsync(new EnterMobileNumberPage(), false);
        }

        private async Task<LocationResult> RequestPermissions()
        {
            try
            {
                var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                var contactPermissionStatus = await Permissions.CheckStatusAsync<Permissions.ContactsRead>();
                if (locationPermissionStatus != PermissionStatus.Granted) {
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
                if(contactPermissionStatus != PermissionStatus.Granted)
                {
                    await Permissions.RequestAsync<Permissions.ContactsRead>();
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
            //SetBusy(false);
            return new LocationResult { Ok = false };
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
