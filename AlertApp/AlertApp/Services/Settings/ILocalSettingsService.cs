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

    }
}
