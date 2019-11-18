using AlertApp.Services.Settings;
using PCLCrypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static PCLCrypto.WinRTCrypto;
using ICryptoTransform = System.Security.Cryptography.ICryptoTransform;

namespace AlertApp.Utils
{
    public class CryptographyHelper
    {
        readonly ILocalSettingsService _localSettingsService;

        public CryptographyHelper(ILocalSettingsService localSettingsService)
        {
            _localSettingsService = localSettingsService;
        }
        public void GenerateKeys(string userPin)
        {
            var asym = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(PCLCrypto.AsymmetricAlgorithm.RsaPkcs1);
            _localSettingsService.SaveApplicationPin(userPin);
            ICryptographicKey key = asym.CreateKeyPair(4096);

            var publicKey = key.ExportPublicKey();
            var privateKey = key.Export();

            var publicKeyString = Convert.ToBase64String(publicKey);
            var privateKeyString = Convert.ToBase64String(privateKey);

            _localSettingsService.SavePublicKey(publicKeyString);

            var encryptedPrivateKey = Encrypt(privateKeyString, userPin);

            _localSettingsService.SavePrivateKey(encryptedPrivateKey);
        }
        public async Task<string> EncryptProfileData(string profileData)
        {
            try
            {
                var fileKey = GetRandomASCIIString(32);
                if (fileKey != null)
                {
                    var base64FileKey = Convert.ToBase64String(fileKey);
                    var userPublicKey = await _localSettingsService.GetPublicKey();
                    var encryptedBase64FileKey = Encrypt(base64FileKey, userPublicKey);

                    _localSettingsService.SaveEncryptedFileKey(encryptedBase64FileKey);

                    return Encrypt(profileData, encryptedBase64FileKey);
                }
            }
            catch (Exception)
            {

            }
            return null;
        }
        public async Task<string> DecryptProfileData(string profileData)
        {
            try
            {
                var fileKey = await _localSettingsService.GetEncryptedFileKey();
                if (fileKey != null)
                {
                    var publicKey = await _localSettingsService.GetPublicKey();
                    var decryptedFileKey = Decrypt(fileKey, publicKey);
                    return Decrypt(profileData, fileKey);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        #region Private Methods
        private static byte[] CreateKey(string password, int keyBytes = 32)
        {
            byte[] salt = new byte[8];
            int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyGenerator.GetBytes(keyBytes);
        }
        private string Encrypt(string clearValue, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = CreateKey(encryptionKey);

                byte[] encrypted = AesEncryptStringToBytes(clearValue, aes.Key, aes.IV);
                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
            }
        }
        private string Decrypt(string encryptedValue, string encryptionKey)
        {
            string iv = encryptedValue.Substring(encryptedValue.IndexOf(';') + 1, encryptedValue.Length - encryptedValue.IndexOf(';') - 1);
            encryptedValue = encryptedValue.Substring(0, encryptedValue.IndexOf(';'));

            return AesDecryptStringFromBytes(Convert.FromBase64String(encryptedValue), CreateKey(encryptionKey), Convert.FromBase64String(iv));
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
        #endregion

    }
}
//test