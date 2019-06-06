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

        public SelectLanguagePageViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
            SetSelectedLanguage();

            MessagingCenter.Subscribe<DialogSelectLanguageViewModel, SelectLanguage>(this, SelectLanguage.Event, (sender, arg) =>
            {
                SetSelectedLanguage();
            });
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
                if(languageService != null)
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
            _localSettingsService.SaveSelectedLanguage(Language.SupportedLanguages[LanguageSelectedIndex].NetLanguageName);            
            CultureInfo ci = new CultureInfo(Language.SupportedLanguages[LanguageSelectedIndex].NetLanguageName);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            Resx.AppResources.Culture = ci;
            await NavigationService.PushAsync(new EnterMobileNumberPage(), false);
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
