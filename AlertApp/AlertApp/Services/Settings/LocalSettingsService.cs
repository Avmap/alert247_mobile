using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using utils = AlertApp.Utils;

namespace AlertApp.Services.Settings
{
    public class LocalSettingsService : ILocalSettingsService
    {
        public async Task<string> GetApplicationPin()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.ApplicationPin);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.ApplicationPin, "");
            }
        }

        public async Task<string> GetAuthToken()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.AuthToken);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.AuthToken, "");
            }
        }

        public async Task<string> GetEncryptedFileKey()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.FileKey);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.FileKey, "");
            }
        }

        public async Task<string> GetEncryptedProfileData()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.ProfileData);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.ProfileData, "");
            }
        }

        public async Task<string> GetPrivateKey()
        {

            try
            {
                return await SecureStorage.GetAsync(utils.Settings.PrivateKey);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.PrivateKey, "");
            }
        }

        public async Task<string> GetPublicKey()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.PublicKey);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.PublicKey, "");
            }
        }

        public string GetSelectedLanguage()
        {
            return Preferences.Get(utils.Settings.SelectedLanguage, Language.Codes.English);
        }

        public async Task<string> GetUserId()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.UserId);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.UserId, "");
            }
        }

        public async void SaveApplicationPin(string pin)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.ApplicationPin, pin);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.ApplicationPin, pin);
            }
        }

        public async void SaveAuthToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.AuthToken, token);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.AuthToken, token);
            }
        }

        public async void SaveEncryptedFileKey(string fileKey)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.FileKey, fileKey);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.FileKey, fileKey);
            }
        }

        public async void SaveEncryptedProfileData(string profileDataJson)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.ProfileData, profileDataJson);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.ProfileData, profileDataJson);
            }
        }

        public async void SavePrivateKey(string key)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.PrivateKey, key);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.PrivateKey, key);
            }
        }

        public async void SavePublicKey(string key)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.PublicKey, key);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.PublicKey, key);
            }
        }

        public void SaveSelectedLanguage(string language)
        {
            Preferences.Set(utils.Settings.SelectedLanguage, language);
        }

        public async void SaveSetting(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(key, value);
            }
        }

        public async void SaveUserId(string userId)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.UserId, userId);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.UserId, userId);
            }
        }
    }
}
