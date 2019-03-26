using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AlertApp.Utils
{
    public class CryptographyHelper
    {
        public static void CreateKeys()
        {
            // Create an instance of the RSA algorithm class  
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
            // Get the public keyy   
            string publicKey = rsa.ToXmlString(false); // false to get the public key   
            string privateKey = rsa.ToXmlString(true); // true to get the private key   

            // Call the encryptText method   
          //  EncryptText(publicKey, "Hello from C# Corner", "encryptedData.dat");

            // Call the decryptData method and print the result on the screen   
           // Console.WriteLine("Decrypted message: {0}", DecryptData(privateKey, "encryptedData.dat"));
        }
    }
}
