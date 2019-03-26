using AlertApp.Infrastructure;
using AlertApp.Pages;
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

        public RegistrationFieldsPageViewModel(IUserProfileService userProfileService,ILocalSettingsService localSettingsService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
        }

        public async void SendUserProfile(Dictionary<string, string> registrationValues)
        {
            try
            {
                SetBusy(true);
                var storedProfile = await _userProfileService.StoreProfile(registrationValues, await _localSettingsService.GetAuthToken(),"test");
                if (storedProfile.IsOk)
                {
                    await NavigationService.PushAsync(new MainPage(), true);
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
