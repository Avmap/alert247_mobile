using AlertApp.Infrastructure;
using AlertApp.MessageCenter;
using AlertApp.Model;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class DialogSelectLanguageViewModel : BaseViewModel
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Commands
        private ICommand _SelectLanguageCommand;
        public ICommand SelectLanguageCommand
        {
            get
            {
                return _SelectLanguageCommand ?? (_SelectLanguageCommand = new Command<Language>(SetSelectedLanguage, (lang) =>
                {
                    return true;
                }));
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<Language> _Languages;
        public ObservableCollection<Language> Languages
        {
            get
            {
                if (_Languages == null)
                {
                    _Languages = new ObservableCollection<Language>(Language.SupportedLanguages);
                }
                return _Languages;
            }
            set
            {
                _Languages = value;
                OnPropertyChanged("Languages");
            }
        }
        
        #endregion


        public DialogSelectLanguageViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
            if (!string.IsNullOrWhiteSpace(_localSettingsService.GetSelectedLanguage()))
            {
                var selectedLanguage = Languages.Where(l => l.NetLanguageName.Equals(_localSettingsService.GetSelectedLanguage())).FirstOrDefault();
                if (selectedLanguage != null)
                    selectedLanguage.Selected = true;

                OnPropertyChanged("Languages");
            }
        }

        private async void SetSelectedLanguage(Language language)
        {
            _localSettingsService.SaveSelectedLanguage(language.NetLanguageName);
            MessagingCenter.Send(this, SelectLanguage.Event, new SelectLanguage { Language = language });
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
