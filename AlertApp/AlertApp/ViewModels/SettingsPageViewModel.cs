using AlertApp.Infrastructure;
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
                }
                else
                {
                    _guardian.StopGuardianService();
                }
            }
        }

        #endregion

        public SettingsPageViewModel()
        {
            _guardian = DependencyService.Get<IGuardian>();
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion   
    }
}
