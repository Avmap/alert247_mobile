using AlertApp.Infrastructure;
using AlertApp.Model.Api;
using AlertApp.Pages;
using AlertApp.Resx;
using AlertApp.Services.Cryptography;
using AlertApp.Services.Profile;
using AlertApp.Services.Registration;
using AlertApp.Services.Settings;
using AlertApp.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlertApp.ViewModels
{
    public class RegistrationFieldsPageViewModel : BaseViewModel
    {
        #region Services
        readonly IUserProfileService _userProfileService;
        readonly ILocalSettingsService _localSettingsService;
        readonly IRegistrationService _registrationService;
        readonly ICryptographyService _cryptographyService;
        #endregion

        #region Properties
        public bool ShowFinishButton => !Busy;
        #endregion

        public RegistrationFieldsPageViewModel(IUserProfileService userProfileService, ILocalSettingsService localSettingsService, IRegistrationService registrationService, ICryptographyService cryptographyService)
        {
            _userProfileService = userProfileService;
            _localSettingsService = localSettingsService;
            _registrationService = registrationService;
            _cryptographyService = cryptographyService;
        }

        public async void SendUserProfile(Dictionary<string, string> registrationValues)
        {
            try
            {
                SetBusy(true);

                var storedProfile = await _userProfileService.StoreProfile(registrationValues, await _localSettingsService.GetAuthToken(), await _localSettingsService.GetPublicKey());
                if (storedProfile.IsOk)
                {
                    Preferences.Set(Settings.HasFinishRegistration, true);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage.Navigation.InsertPageBefore(new MainPage(), Application.Current.MainPage.Navigation.NavigationStack.First());
                        Application.Current.MainPage.Navigation.PopToRootAsync();
                    });

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

        public async Task<bool> UpdateUserProfile(Dictionary<string, string> registrationValues)
        {
            try
            {
                SetBusy(true);
                var storedProfile = await _userProfileService.StoreProfile(registrationValues, await _localSettingsService.GetAuthToken(), await _localSettingsService.GetPublicKey());
                if (storedProfile.IsOk)
                {
                    return true;
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
            return false;
        }

        public async Task<Response<RegistrationField[]>> GetRegistrationFieldsAsync()
        {
            var token = await _localSettingsService.GetAuthToken();
            var registrationFiedsResult = await _registrationService.GetRegistrationFields(token);
            return registrationFiedsResult;
        }

        public async Task<Response<GetProfileResponse>> GetUserProfileAsync()
        {
            var token = await _localSettingsService.GetAuthToken();
            var userId = await _localSettingsService.GetUserId();
            var profle = await _userProfileService.GetProfile(token, userId);
            return profle;
        }

        public async Task<Dictionary<string, string>> DecryptProfileAsync(string encryptedProfileData)
        {
            var json = await _cryptographyService.DecryptProfileData(encryptedProfileData);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        #region BaseViewModel

        public override void SetBusy(bool isBusy)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Busy = isBusy;
                OnPropertyChanged("ShowFinishButton");
            });
        }
        #endregion
    }
}
