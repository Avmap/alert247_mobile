using AlertApp.Model;
using AlertApp.Services.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

using System.Text;
using System.Threading.Tasks;
using PCLCrypto;
using System.Diagnostics;
using static PCLCrypto.WinRTCrypto;
using ICryptoTransform = System.Security.Cryptography.ICryptoTransform;
using AlertApp.Model.Api;
using Newtonsoft.Json;

namespace AlertApp.Services.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        #region Services
        readonly ILocalSettingsService _localSettingsService;
        #endregion

        public CryptographyService(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
        }



        public void GenerateKeys(string userPin)
        {
            _localSettingsService.SaveApplicationPin(userPin);
            var asymmAlgorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);

            try
            {
                ICryptographicKey key = asymmAlgorithm.CreateKeyPair(4096);

                var privateKeyBytes = key.Export(CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);
                var publicKeyBytes = key.ExportPublicKey(CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);

                var privateKeyBase64 = Convert.ToBase64String(privateKeyBytes);
                var publicKeyBase64 = Convert.ToBase64String(publicKeyBytes);

                //save public key
                _localSettingsService.SavePublicKey(publicKeyBase64);

                var encryptedPrivateKey = AesEncrypt(privateKeyBase64, userPin);

                //save private key
                _localSettingsService.SavePrivateKey(encryptedPrivateKey);
            }
            catch (Exception ex)
            {
                Debug.Fail("ERROR: " + ex.Message.ToString());
            }




            //too slow method with RSACryptoServiceProvider
            //using (var rsa = new RSACryptoServiceProvider(4096))
            //{
            //    _localSettingsService.SaveApplicationPin(userPin);

            //    rsa.PersistKeyInCsp = false;

            //    var publicKey = rsa.ToXmlString(false);
            //    var privateKey = rsa.ToXmlString(true);

            //    _localSettingsService.SavePublicKey(publicKey);
            //    var encryptedPrivateKey = AesEncrypt(privateKey, userPin);

            //    _localSettingsService.SavePrivateKey(encryptedPrivateKey);
            //}
        }
        public async Task<EncryptedProfileData> EncryptProfileData(string profileData)
        {
            try
            {
                //Create algorithm
                var asymmAlgorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);
                //get base 64 public key from settings
                var publicKeyBase64 = await _localSettingsService.GetPublicKey();
                //import public key
                ICryptographicKey publicKeyEncryptor = asymmAlgorithm.ImportPublicKey(Convert.FromBase64String(publicKeyBase64), CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);

                byte[] profileDataBytes = Encoding.UTF8.GetBytes(profileData);

                //encrypt profile data with rsa and user public key
                byte[] ciphertext = WinRTCrypto.CryptographicEngine.Encrypt(publicKeyEncryptor, profileDataBytes);

                //convert encrypted profile data to base 64
                string cipheredDataBase64 = Convert.ToBase64String(ciphertext);


                return new EncryptedProfileData { Data = cipheredDataBase64, PublicKey = publicKeyBase64 };


                //using (var rsa = new RSACryptoServiceProvider(4096))
                //{
                //    rsa.PersistKeyInCsp = false;
                //    var fileKey = GetRandomASCIIString(32);
                //    if (fileKey != null)
                //    {
                //        //generate base64 filekey
                //       // var base64FileKey = Convert.ToBase64String(fileKey);

                //        //get publickey
                //        var publicKey = await _localSettingsService.GetPublicKey();

                //        rsa.FromXmlString(publicKey);

                //        byte[] profileDataBytes = Encoding.UTF8.GetBytes(profileData);
                //        byte[] cipherProfile= rsa.Encrypt(profileDataBytes, false);
                //        string base64EncryptedProfile = Convert.ToBase64String(cipherProfile);

                //       // var encryptedBase64FileKey = AesEncrypt(base64FileKey, publicKey);

                //        //_localSettingsService.SaveEncryptedFileKey(encryptedBase64FileKey);


                //        var publicKeyBytes = System.Text.Encoding.UTF8.GetBytes(publicKey);

                //        return new EncryptedProfileData { Data = base64EncryptedProfile, PublicKey = Convert.ToBase64String(publicKeyBytes) };
                //    }

            }
            catch (Exception ex)
            {
                var test = ex.Message;
            }
            return null;
        }
        public async Task<string> DecryptProfileData(string profileDataBase64)
        {
            try
            {

                var enctyptedPrivateKey = await _localSettingsService.GetPrivateKey();
                var userPin = await _localSettingsService.GetApplicationPin();

                var decryptedPrivateKey = AesDecrypt(enctyptedPrivateKey, userPin);

                var privateKey = Convert.FromBase64String(decryptedPrivateKey);

                var asymmAlgorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);
                ICryptographicKey privateKeyDecryptor = asymmAlgorithm.ImportKeyPair(privateKey, CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);

                var encryptedProfileData = Convert.FromBase64String(profileDataBase64);

                byte[] plaintext = WinRTCrypto.CryptographicEngine.Decrypt(privateKeyDecryptor, encryptedProfileData);

                string decryptedProfileData = Encoding.UTF8.GetString(plaintext);
                return decryptedProfileData;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
        public string AesEncrypt(string clearValue, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = CreateKey(encryptionKey);

                byte[] encrypted = AesEncryptStringToBytes(clearValue, aes.Key, aes.IV);
                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
            }
        }
        public string AesDecrypt(string encryptedValue, string encryptionKey)
        {
            string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
            encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

            return AesDecryptStringFromBytes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey), Convert.FromBase64String(iv));
        }

        #region Private Methods
        private static byte[] CreateKey(string password, int keyBytes = 32)
        {
            byte[] salt = new byte[8];
            int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyGenerator.GetBytes(keyBytes);
        }

        private string AesDecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException($"{nameof(cipherText)}");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException($"{nameof(key)}");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException($"{nameof(iv)}");

            string plaintext = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                    plaintext = streamReader.ReadToEnd();

            }
            return plaintext;
        }
        private byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException($"{nameof(plainText)}");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException($"{nameof(key)}");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException($"{nameof(iv)}");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, encryptor, System.Security.Cryptography.CryptoStreamMode.Write))
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    encrypted = memoryStream.ToArray();
                }
            }
            return encrypted;
        }
        private byte[] GetRandomUnicodeString(int length, int maxValue, Predicate<int> valueFilter)
        {
            byte[] seedBuff = new byte[4];
            byte[] charBuff;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(seedBuff); // The array is now filled with cryptographically strong random bytes.

            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sw = new StreamWriter(ms, new UTF8Encoding(false, false)))
                {
                    var random = new Random(BitConverter.ToInt32(seedBuff, 0));

                    for (int i = 0; i < length; i++)
                    {
                        int temp = random.Next(maxValue); //should we cap? 

                        while (!valueFilter(temp))
                            temp = random.Next(maxValue);

                        sw.Write((char)temp);
                    }
                }
                charBuff = ms.ToArray();
            }
            return charBuff;
            // return new System.Text.UTF8Encoding(false, false).GetString(charBuff);
        }
        private byte[] GetRandomASCIIString(int length)
        {
            return GetRandomUnicodeString(length, 0x7E, o => o >= 0x21 && o <= 0x7E);
        }

        public async Task<string> DecryptFileKey(string encryptedFileKey)
        {
            var privateKey = await _localSettingsService.GetPrivateKey();
            var pin = await _localSettingsService.GetApplicationPin();
            var decryptedPrivate = AesDecrypt(privateKey, pin);
            return AesDecrypt(encryptedFileKey, privateKey);
        }

        public async Task<AlertRecipient> GetAlertRecipient(string senderProfileData, Contact recipient)
        {
            try
            {
                //Create algorithm
                var asymmAlgorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);
                //create filekey
                var fileKey = GetRandomASCIIString(32);
                //make base64 file key
                var base64FileKey = Convert.ToBase64String(fileKey);

                var aesProfileData = AesEncrypt(senderProfileData, base64FileKey);

                //import public key
                ICryptographicKey publicKeyEncryptor = asymmAlgorithm.ImportPublicKey(Convert.FromBase64String(recipient.PublicKey), CryptographicPublicKeyBlobType.Pkcs1RsaPublicKey);

                byte[] encryptedFileKey = Encoding.UTF8.GetBytes(base64FileKey);

                //encrypt profile data with rsa and user public key
                byte[] ciphertext = WinRTCrypto.CryptographicEngine.Encrypt(publicKeyEncryptor, encryptedFileKey);

                //convert encrypted profile data to base 64
                string cipheredDataBase64 = Convert.ToBase64String(ciphertext);

                return new AlertRecipient { Cellphone = recipient.Cellphone, Filekey = cipheredDataBase64, Profiledata = aesProfileData };
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Dictionary<string, string>> GetAlertSenderProfileData(string encryptedProfileData, string fileKey)
        {
            try
            {
                var enctyptedPrivateKey = await _localSettingsService.GetPrivateKey();
                var userPin = await _localSettingsService.GetApplicationPin();

                var decryptedPrivateKey = AesDecrypt(enctyptedPrivateKey, userPin);

                var privateKey = Convert.FromBase64String(decryptedPrivateKey);

                var asymmAlgorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);
                ICryptographicKey privateKeyDecryptor = asymmAlgorithm.ImportKeyPair(privateKey, CryptographicPrivateKeyBlobType.Pkcs1RsaPrivateKey);

                var fileKeyString = Convert.FromBase64String(fileKey);

                byte[] plaintextFileKey = WinRTCrypto.CryptographicEngine.Decrypt(privateKeyDecryptor, fileKeyString);

                string descryptedProfileData = AesDecrypt(encryptedProfileData, Encoding.UTF8.GetString(plaintextFileKey));

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(descryptedProfileData);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        public async Task<bool> ChangePin(string newPin)
        {
            try
            {
                var oldPin = await _localSettingsService.GetApplicationPin();
                var encryptedPrivateKey = await _localSettingsService.GetPrivateKey();
                var decryptedPrivateKey = AesDecrypt(encryptedPrivateKey, oldPin);

                var newEncryptedPrivateKey = AesEncrypt(decryptedPrivateKey, newPin);

                
                _localSettingsService.SavePrivateKey(newEncryptedPrivateKey);

                _localSettingsService.SaveApplicationPin(newPin);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
