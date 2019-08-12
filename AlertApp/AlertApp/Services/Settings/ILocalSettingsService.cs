using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Settings
{
    public interface ILocalSettingsService
    {
        void SaveAuthToken(string token);
        void SaveSelectedLanguage(string language);
        void SaveApplicationPin(string pin);
        string GetSelectedLanguage();
        Task<string> GetAuthToken();
        Task<string> GetApplicationPin();
        Task<string> GetPrivateKey();
        Task<string> GetPublicKey();
        void SavePrivateKey(string key);
        void SavePublicKey(string key);
        void SaveEncryptedFileKey(string key);
        Task<string> GetEncryptedFileKey();
        void SaveUserId(string userId);
        Task<string> GetUserId();
        void SaveEncryptedProfileData(string profileDataJson);
        Task<string> GetEncryptedProfileData();

        void SaveFirebaseToken(string token);
        Task<string> GetFirebaseToken();

        bool GetAlwaysOn();
        void SetAlwaysOn(bool value);
    }
}
