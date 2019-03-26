using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Services.Community;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlertApp.ViewModels
{
    public class DependandsPageViewModel : BaseViewModel
    {
        #region Services
        readonly ICommunityService _communityService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties
        private Contact[] _Dependands;
        public Contact[] Dependands
        {
            get { return _Dependands; }
            set
            {
                _Dependands = value;
                OnPropertyChanged("Dependands");
            }
        }

        #endregion

        public DependandsPageViewModel(ICommunityService communityService, ILocalSettingsService localSettingsService)
        {
            _communityService = communityService;
            _localSettingsService = localSettingsService;
            GetDependands();
        }

        private async void GetDependands()
        {
            SetBusy(true);
            var result = await _communityService.GetDependands("123123");
            if (result != null && result.IsOk)
            {
                Dependands = result.Result.Dependants;
            }
            SetBusy(false);
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
