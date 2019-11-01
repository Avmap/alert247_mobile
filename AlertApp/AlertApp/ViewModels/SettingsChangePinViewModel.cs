using AlertApp.Infrastructure;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SettingsChangePinViewModel : BaseViewModel
    {
        #region Services        
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties
        private bool _NewPinLayoutVisible;

        public bool NewPinLayoutVisible
        {
            get { return _NewPinLayoutVisible; }
            set
            {
                _NewPinLayoutVisible = value;
                OnPropertyChanged("CurrentPinLayoutVisible");
                OnPropertyChanged("NewPinLayoutVisible");
            }
        }

        private bool _CanFinish;

        public bool CanFinish
        {
            get { return _CanFinish; }
            set
            {
                _CanFinish = value;
                OnPropertyChanged("CanFinish");                
            }
        }

        #endregion

        #region Commands

        private ICommand _ChangePinCommand;
        public ICommand ChangePinCommand
        {
            get
            {
                return _ChangePinCommand ?? (_ChangePinCommand = new Command(ChangePin, () =>
                {
                    return !Busy;
                }));
            }
        }
  
        
        #endregion


        public bool CurrentPinLayoutVisible => !NewPinLayoutVisible;

        public SettingsChangePinViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
        }

        public Task<string> GetApplicationPin()
        {
            return _localSettingsService.GetApplicationPin();
        }

        private async void ChangePin()
        {
            
        }

        public override void SetBusy(bool isBusy)
        {

        }
    }
}
