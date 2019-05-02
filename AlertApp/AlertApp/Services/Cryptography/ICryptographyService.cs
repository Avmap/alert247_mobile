using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Cryptography
{
    public interface ICryptographyService
    {
        void GenerateKeys(string userPin);
        Task<string> EncryptProfileData(string profileData);
        Task<string> DecryptProfileData(string profileData);
    }
}
