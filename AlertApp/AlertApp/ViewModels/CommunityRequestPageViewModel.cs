using AlertApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.ViewModels
{
    public class CommunityRequestPageViewModel : BaseViewModel
    {
        public CommunityRequestPageViewModel()
        {

        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
