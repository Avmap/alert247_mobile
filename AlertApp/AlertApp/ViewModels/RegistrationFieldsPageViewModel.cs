using AlertApp.Infrastructure;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Profile;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.ViewModels
{
    public class RegistrationFieldsPageViewModel : BaseViewModel
    {
        #region Services
        readonly IUserProfileService _userProfileService;
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        #region Properties
        public bool ShowFinishButton => !Busy;
        #endregion

        public RegistrationFieldsPageViewModel(IUserProfileService userProfileService, ILocalSettingsService localSettingsService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
        }

        public async void SendUserProfile(Dictionary<string, string> registrationValues)
        {
            try
            {
                SetBusy(true);

                var storedProfile = await _userProfileService.StoreProfile(registrationValues, await _localSettingsService.GetAuthToken(), await _localSettingsService.GetPublicKey());
                if (storedProfile.IsOk)
                {
                    //await NavigationService.PushAsync(new MainPage(), true);
                    await NavigationService.PushAsync(new MainTabbedPage(), true);
                }
                else if (!storedProfile.IsOnline)
                {
                    showOKMessage(AppResources.Error, "Please check your internet connection.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            SetBusy(false);
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            this.Busy = isBusy;
            OnPropertyChanged("ShowFinishButton");
        }
        #endregion
    }
}
