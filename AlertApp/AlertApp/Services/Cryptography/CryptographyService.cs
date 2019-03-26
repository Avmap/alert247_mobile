using AlertApp.Model;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AlertApp.Services.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        readonly ILocalSettingsService _localSettingsService;
        public CryptographyService(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
        }

        public async Task<RsaKeys> CreateKeys(string pin)
        {
            // Create an instance of the RSA algorithm class  
            //  RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            // Get the public keyy   
            string publicKey = rsa.ToXmlString(false); // false to get the public key   
            string privateKey = rsa.ToXmlString(true); // true to get the private key   
            
            _localSettingsService.SavePrivateKey(privateKey);
            _localSettingsService.SavePublicKey(publicKey);
        
            // Call the encryptText method   
            EncryptText(publicKey, "Hello from C# Corner", "encryptedData.dat");

            return null;
        }
      
        static void EncryptText(string publicKey, string text, string fileName)
        {
            // Convert the text to an array of bytes   
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(text);

            // Create a byte array to store the encrypted data in it   
            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the rsa pulic key   
                rsa.FromXmlString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, false);
            }

            Console.WriteLine("Data has been encrypted");
        }
        
        static string DecryptData(string privateKey, string data)
        {
            // read the encrypted bytes from the file   
     
            // Create an array to store the decrypted data in it   
            byte[] decryptedData;
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToDecrypt = byteConverter.GetBytes(data);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the private key of the algorithm   
                rsa.FromXmlString(privateKey);
                decryptedData = rsa.Decrypt(dataToDecrypt, false);
            }

            // Get the string value from the decryptedData byte array               
            return byteConverter.GetString(decryptedData);
        }

        public string EncryptAES(string data, string pin)
        {
            throw new NotImplementedException();
        }

        public string DecryptAES(string data, string pin)
        {
            throw new NotImplementedException();
        }
    }
}
