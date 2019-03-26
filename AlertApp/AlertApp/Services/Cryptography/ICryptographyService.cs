using AlertApp.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Cryptography
{
    public interface ICryptographyService
    {
        Task<RsaKeys> CreateKeys(string pin);        
        string EncryptAES(string data, string pin);
        string DecryptAES(string data, string pin);
    }
}
