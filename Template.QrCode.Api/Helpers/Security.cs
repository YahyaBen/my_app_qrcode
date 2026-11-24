using System;

using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace Template.QrCode.Api.Helpers
{
    public static class Security
    {


        #region CryptCodeQr
        public static string EncryptPdf(string imput, string Location, string CryptCertificatePublic, string HashCertificatePrivate,string HashCertificatePrivatePassword)
        {
            try
            {
                var csp = new RSACryptoServiceProvider();
                X509Certificate2 x = X509CertificateLoader.LoadCertificateFromFile(Location + CryptCertificatePublic);
                var rsapublickey = RSACertificateExtensions.GetRSAPublicKey(x);
              
                // public key 
                var pubKey = rsapublickey.ExportParameters(false);

                string CryptedString = EncryptAesManaged(imput, out byte[] KEY, out byte[] IV);

                string SymetricKeyCrypted = EncryptValueWithRsa(KEY, pubKey);
                string VecteurInfo = Convert.ToBase64String(IV);


            
                X509Certificate2 cert2 = X509CertificateLoader.LoadPkcs12FromFile(
                Location + HashCertificatePrivate,
               HashCertificatePrivatePassword,
                X509KeyStorageFlags.Exportable);


                var pubKey2 = rsapublickey.ExportParameters(false);
                //csp.ImportParameters(pubKey);

                //string XmlString = csp.ToXmlString(true);
                var csp2 = new RSACryptoServiceProvider();
                csp2.FromXmlString(cert2.PrivateKey.ToXmlString(true)); // pour decryter
                var privKey2 = csp2.ExportParameters(true);


                ASCIIEncoding ByteConverter = new ASCIIEncoding();



                // Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] originalData = Convert.FromBase64String(CryptedString);
                byte[] signedData;
                signedData = HashAndSignBytes(originalData, privKey2);

                // Verify the data and display the result to the
                // console.

                return Convert.ToBase64String(signedData) + ";" + SymetricKeyCrypted + ";" + VecteurInfo + ";" + CryptedString;
            }
            catch (Exception ex)
            {
                return null;
                throw;
            }
        }

        public static string DecryptPdf(string input, string Location)
        {
            try
            {
                string[] List = input.Split(';');

                string decrypted = "";
                var csp = new RSACryptoServiceProvider();
                X509Certificate2 x2 = X509CertificateLoader.LoadCertificateFromFile(Location + "Cert2 Public with DER encoded binary.cer");
                var rsapublickey2 = RSACertificateExtensions.GetRSAPublicKey(x2);
                X509Certificate2 cert = X509CertificateLoader.LoadPkcs12FromFile(
                 Location + "Cert1 Private key.pfx",
                 "123***s",
                 X509KeyStorageFlags.Exportable);

                //RSACryptoServiceProvider publicKeyProvider = (RSACryptoServiceProvider)x2.PublicKey.Key;

                //RSACryptoServiceProvider privateKeyProvider = (RSACryptoServiceProvider)cert.PrivateKey;


                var pubKey =  RSACertificateExtensions.GetRSAPublicKey(x2);
                var privKey = RSACertificateExtensions.GetRSAPrivateKey(cert);

                byte[] DecryptedsymetricKey = DecryptValuetWithRsa(List[1], privKey.ExportParameters(true));

                var DecryptedValue = Security.DecryptAes(Convert.FromBase64String(List[3]), DecryptedsymetricKey, Convert.FromBase64String(List[2]));
                ASCIIEncoding ByteConverter = new ASCIIEncoding();



                // Create byte arrays to hold original, encrypted, and decrypted data.
                byte[] originalData = ByteConverter.GetBytes(DecryptedValue);
                var T = VerifySignedHash(originalData, Convert.FromBase64String(List[0]), pubKey.ExportParameters(false));
                if (T)
                    decrypted = DecryptedValue;
                return decrypted;
            }
            catch (Exception ex)
            {
                return null;
                //throw;
            }
        }

        public static string EncryptAesManaged(string raw, out byte[] Key, out byte[] IV)
        {

            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    string encrypted = EncryptAes(raw, aes.Key, aes.IV);
                    // Print encrypted string    
                    //Console.WriteLine($"Encrypted data: {System.Text.Encoding.UTF8.GetString(encrypted)}");
                    // Decrypt the bytes to a string.    
                    //string decrypted = Decrypt(encrypted, aes.Key, aes.IV);
                    // Print decrypted string. It should be same as raw data    
                    //Console.WriteLine($"Decrypted data: {decrypted}");
                    Key = aes.Key;
                    IV = aes.IV;
                    return encrypted;
                }
            }
            catch (Exception exp)
            {
                Key = null;
                IV = null;
                return null;

            }

        }


        public static string EncryptAes(string plainText, byte[] Key, byte[] IV)
        {
            string encrypted;
            // Create a new AesManaged.    
            using (AesManaged aes = new AesManaged())
            {
                // Create encryptor    
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                // Create MemoryStream    
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        // Create StreamWriter and write data to a stream    
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            // Return encrypted data    
            return encrypted;
        }

        public static string DecryptAes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }

        public static string EncryptValueWithRsa(byte[] txtToEncrypt, RSAParameters lRsaParam)
        {
            try
            {
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
               
                //load key
                csp.ImportParameters(lRsaParam);
                //var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(txtToEncrypt);
                
                //paddig + ecrypt data
                var bytesCypherText = csp.Encrypt(txtToEncrypt,false);
                string Value = Convert.ToBase64String(bytesCypherText);
                return Value;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static byte[] DecryptValuetWithRsa(string txtCrypted, RSAParameters privKey)
        {

            var bytesCypherText = Convert.FromBase64String(txtCrypted);

            //we want to decrypt, therefore we need a csp and load our private key

            RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

            csp.ImportParameters(privKey);

            //decrypt and strip pkcs#1.5 padding
            var bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

            //get our original plainText back...
            return bytesPlainTextData;
        }


        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA256
                // to specify the hashing algorithm.
                return RSAalg.SignData(DataToSign, SHA256.Create());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA256
                // to specify the hashing algorithm.
                return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }
        #endregion
        public static string DescryptID(string idE)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(idE))
                {
                    string lKeyID = "9f7Wo4lFFJfWIYBfN0P/8uFxSSPaiZN++bX/Zf6N1K4=";
                    string lIVID = "ZaKO5vgBL0oa9cShQIcjjg==";

                    using (RijndaelManaged myRijndael = new RijndaelManaged())
                    {

                        myRijndael.BlockSize = 128;
                        myRijndael.Mode = CipherMode.CBC;
                        return DecryptStringFromBytes(Convert.FromBase64String(idE), Convert.FromBase64String(lKeyID), Convert.FromBase64String(lIVID));

                    }
                }
                return string.Empty;
            }
            catch (Exception e)
            {

                return string.Empty;
            }

        }
        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}
