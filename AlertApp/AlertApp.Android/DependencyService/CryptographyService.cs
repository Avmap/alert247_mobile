using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlertApp.Model;
using AlertApp.Services.Cryptography;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Security;
using Java.Security.Spec;
using Javax.Crypto;
using Javax.Crypto.Spec;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertApp.Droid.DependencyService.CryptographyService))]
namespace AlertApp.Droid.DependencyService
{
    public class CryptographyService// : ICryptographyService
    {
        public async Task<RsaKeys> CreateKeys(string pin)
        {
            KeyPairGenerator keyGen = KeyPairGenerator.GetInstance("RSA");
            keyGen.Initialize(4096);
            KeyPair keys = keyGen.GenKeyPair();
            if (keys != null)
            {
                string privateKeyBase64 = Base64.EncodeToString(keys.Private.GetEncoded(), Base64Flags.Default);
                string publicKeyBase64 = Base64.EncodeToString(keys.Public.GetEncoded(), Base64Flags.Default);
                var privateAes = EncryptAES(privateKeyBase64,pin);                
                return new RsaKeys { PrivateKey = privateAes, PublicKey = publicKeyBase64 };
            }

            return null;
        }

        public  string EncryptAES(string data, string pin)
        {
            try
            {
                //Get Cipher Instance
                Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5Padding");

                //Create SecretKeySpec
                SecureRandom random = new SecureRandom();
                byte[] salt = new byte[16];
                //random.NextBytes(salt);

                SecretKeyFactory factory = SecretKeyFactory.GetInstance("PBKDF2WithHmacSHA1");
                IKeySpec spec = new PBEKeySpec(pin.ToCharArray(), salt, 65536, 256);
                ISecretKey tmp = factory.GenerateSecret(spec);
                SecretKeySpec secret = new SecretKeySpec(tmp.GetEncoded(), "AES");

                // Generating IV.
                byte[] IV = new byte[16];
                SecureRandom randomIV = new SecureRandom();
                //randomIV.NextBytes(IV);
                //Create IvParameterSpec
                IvParameterSpec ivSpec = new IvParameterSpec(IV);

                //Initialize Cipher for ENCRYPT_MODE
                cipher.Init(CipherMode.EncryptMode, secret, ivSpec);

                //Perform Encryption
                byte[] cipherText = cipher.DoFinal(System.Text.Encoding.UTF8.GetBytes(data));

                return Base64.EncodeToString(cipherText, Base64Flags.Default); ;
            }
            catch (Exception e)
            {
                //System.out.println("Error while encrypting: " + e.toString());
            }
            return null;
        }

        public string DecryptAES(string strToDecrypt, string pin)
        {
            try
            {
                byte[] salt = new byte[16];
                byte[] iv = new byte[16];
                IvParameterSpec ivspec = new IvParameterSpec(iv);

                SecretKeyFactory factory = SecretKeyFactory.GetInstance("PBKDF2WithHmacSHA1");
                IKeySpec spec = new PBEKeySpec(pin.ToCharArray(), salt, 65536, 256);
                ISecretKey tmp = factory.GenerateSecret(spec);
                SecretKeySpec secret = new SecretKeySpec(tmp.GetEncoded(), "AES");

                Cipher cipher = Cipher.GetInstance("AES/CBC/PKCS5PADDING");
                cipher.Init(CipherMode.DecryptMode, secret, ivspec);
                byte[] cipherText = cipher.DoFinal(System.Text.Encoding.UTF8.GetBytes(strToDecrypt));
                return System.Text.Encoding.UTF8.GetString(cipherText);
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        
    }
}