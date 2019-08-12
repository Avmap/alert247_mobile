using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Services.Community;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class MyCommunityPageViewModel : BaseViewModel
    {
        #region Services
        readonly ICommunityService _communityService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties
        private Contact[] _Community;
        public Contact[] Community
        {
            get { return _Community; }
            set
            {
                _Community = value;
                OnPropertyChanged("Community");
            }
        }

        #endregion

        #region Commands
        private ICommand _OpenContactsScreenCommand;
        public ICommand OpenContactsScreenCommand
        {
            get
            {
                return _OpenContactsScreenCommand ?? (_OpenContactsScreenCommand = new Command(OpenAddContactsPage, () =>
                {
                    return !Busy;
                }));
            }
        }


        #endregion
        public MyCommunityPageViewModel(ICommunityService communityService, ILocalSettingsService localSettingsService)
        {
            _communityService = communityService;
            _localSettingsService = localSettingsService;
            GetCommynity();
        }

        private async void GetCommynity()
        {
            SetBusy(true);
            var result = await _communityService.GetCommunity("123123");
            if (result != null && result.IsOk)
            {
                Community = result.Result.MyCommunity;
            }
            SetBusy(false);
        }
        private async void OpenAddContactsPage()
        {
            await NavigationService.PushAsync(new AddContactPage(), true);
        }

        #region BaseViewModel
        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }
        #endregion
    }
}
