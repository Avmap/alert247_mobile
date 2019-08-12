using AlertApp.Infrastructure;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Services
        readonly IGuardian _guardian;
        readonly ILocalSettingsService _localSettingsService;
        #endregion
        #region Properties
        private bool _AllwaysOn;

        public bool AllwaysOn
        {
            get { return _AllwaysOn; }
            set
            {
                _AllwaysOn = value;
                if (_AllwaysOn)
                {
                    _guardian.StartGuardianService();
                    _localSettingsService.SetAlwaysOn(true);                                        
                }
                else
                {
                    _guardian.StopGuardianService();
                    _localSettingsService.SetAlwaysOn(false);
                }
            }
        }

        #endregion

        public SettingsPageViewModel(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
            _guardian = DependencyService.Get<IGuardian>();
            AllwaysOn = _localSettingsService.GetAlwaysOn();
        }
   
        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion   
    }
}
