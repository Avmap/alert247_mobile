﻿using AlertApp.Model;
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
                //if (string.IsNullOrWhiteSpace(res))
                //{
                //    res = Preferences.Get(utils.Settings.ApplicationPin, "");
                //}
                //return res;
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
            string mykey="";
            try
            {
                mykey = await SecureStorage.GetAsync(utils.Settings.PrivateKey);
                if (String.IsNullOrEmpty(mykey))
                    mykey = Preferences.Get(utils.Settings.PrivateKey, "");

            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                mykey =  Preferences.Get(utils.Settings.PrivateKey, "");
            }
            return mykey;
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

        public async void SaveFirebaseToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.FirebaseToken, token);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.FirebaseToken, token);
            }
        }

        public async Task<string> GetFirebaseToken()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.FirebaseToken);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.FirebaseToken, "");
            }
        }

        public bool GetAlwaysOn()
        {
            return Preferences.Get(utils.Settings.AlwaysOn, false);
        }

        public void SetAlwaysOn(bool value)
        {
            Preferences.Set(utils.Settings.AlwaysOn, value);
        }

        public void SaveSendLocationSetting(bool sendLocation)
        {
            Preferences.Set(utils.Settings.SendLocation, sendLocation);
        }

        public bool GetSendLocationSetting()
        {
            return Preferences.Get(utils.Settings.SendLocation, false);
        }

        public void SaveAppHasRunSetting(bool firstRun)
        {
            Preferences.Set(utils.Settings.AppHasRun, firstRun);
        }

        public bool GetAppHasRunSetting()
        {
            return Preferences.Get(utils.Settings.AppHasRun, false);
        }

        public int GetCellPhoneNotificationId(string cellphone)
        {
            return Preferences.Get(cellphone + "_", 0);
        }

        public bool GetFallDetecion()
        {
            return Preferences.Get(utils.Settings.FallDetecion, false);
        }

        public void SetFallDetection(bool value)
        {
            Preferences.Set(utils.Settings.FallDetecion, value);
        }

        public async Task SaveMobilePhone(string mobilePhone)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.MobileNumber, mobilePhone);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.MobileNumber, mobilePhone);
            }
        }

        public async Task<string> GetMobilePhone()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.MobileNumber);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.MobileNumber, "");
            }
        }

        public async Task SaveName(string name)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.Name, name);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.Name, name);
            }
        }

        public async Task SaveSurname(string surname)
        {
            try
            {
                await SecureStorage.SetAsync(utils.Settings.Surname, surname);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                Preferences.Set(utils.Settings.Surname, surname);
            }
        }

        public async Task<string> GetName()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.Name);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.Name, "");
            }
        }

        public async Task<string> GetSurname()
        {
            try
            {
                return await SecureStorage.GetAsync(utils.Settings.Surname);
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
                return Preferences.Get(utils.Settings.Surname, "");
            }
        }

        public LocalSettingsService()
        {

        }
    }
}
