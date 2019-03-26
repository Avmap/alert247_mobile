using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Services.Profile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Services
        readonly IUserProfileService _userProfileService;
        #endregion

        #region Commands

        private ICommand _SosCommand;
        public ICommand SosCommand
        {
            get
            {
                return _SosCommand ?? (_SosCommand = new Command(OpenSendAlertScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private ICommand _OpenContactsScreen;
        public ICommand OpenContactsScreen
        {
            get
            {
                return _OpenContactsScreen ?? (_OpenContactsScreen = new Command(NavigateToContactScreen, () =>
                {
                    return !Busy;
                }));
            }
        }

        private async void NavigateToContactScreen()
        {
            await NavigationService.PushAsync(new ManageContactsPage(),true);
        }

        #endregion

        public MainPageViewModel(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        private async void OpenSendAlertScreen()
        {
            await NavigationService.PushAsync(new SendingAlertPage(Model.AlertType.UserAlert),true);
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
        }

        #endregion        
    }
}
