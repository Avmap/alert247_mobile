using AlertApp.Infrastructure;
using AlertApp.Model;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class EnterMobileNumberPageViewModel : BaseViewModel
    {
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

        #region Properties
        public string CountryPrefix { get; set; }
        public Language SelectedLanguage
        {
            get
            {
                try
                {
                    var selectedLang = Preferences.Get(Settings.SelectedLanguage, Language.Codes.English);
                    return Language.SupportedLanguages.Where(l => l.NetLanguageName.Equals(selectedLang)).FirstOrDefault();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public string Mobile { get; set; }        
        #endregion

        public EnterMobileNumberPageViewModel()
        {
            var selectedLang = Preferences.Get(Settings.SelectedLanguage, Language.Codes.English);
            CountryPrefix =  Language.SupportedLanguages.Where(l => l.NetLanguageName.Equals(selectedLang)).FirstOrDefault().CountryMobilePrefix;
        }

        public async void Continue()
        {
            if (!string.IsNullOrWhiteSpace(Mobile))
            {
                string message = AppResources.SmsVerificationMessage + " " + String.Format("{0}{1}", CountryPrefix, Mobile);

                var confirm = await showAlertMessage(AppResources.Verification, message, AppResources.ContinueDialogButton, AppResources.Cancel);
                if (confirm)
                {
                    await NavigationService.PushAsync(new EnterActivationCodePage(String.Format("{0}{1}", CountryPrefix, Mobile)), false);
                }

            }
            else
            {
                showOKMessage(AppResources.Warning, AppResources.WarningFillNumber);
            }
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion
    }
}
