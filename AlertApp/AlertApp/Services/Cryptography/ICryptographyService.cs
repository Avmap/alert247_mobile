using AlertApp.Model;
using AlertApp.Model.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Cryptography
{
    public interface ICryptographyService
    {
        void GenerateKeys(string userPin);
        Task<EncryptedProfileData> EncryptProfileData(string profileData);
        Task<string> DecryptProfileData(string profileDataBase64);
        Task<string> DecryptFileKey(string encryptedFileKey);
        string AesDecrypt(string encrypteddata,string key);
        string AesEncrypt(string plainText, string key);
        Task<AlertRecipient> GetAlertRecipient(string senderProfileData,Contact recipient);
    }
}
