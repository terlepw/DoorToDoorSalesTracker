using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DoorToDoorLibrary.Logic
{
    /// <summary>
    /// Used to manage password verification, hash generation, salt generation, and encryption
    /// </summary>
    public class PasswordManager
    {
        /// <summary>
        /// Determines how long it takes to validate a password
        /// </summary>
        private const int WorkFactor = 200;

        /// <summary>
        /// Determines the size of the salt used to hash the password
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Holds the salt value used in hashing the password
        /// </summary>
        public string Salt { get; private set; }

        /// <summary>
        /// Holds the password hash value
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Use when Registering a new user
        /// </summary>
        /// <param name="password">The password entered by the user</param>
        public PasswordManager(string password)
        {
            Salt = GenerateSalt(password, SaltSize, WorkFactor);
            Hash = GenerateHash(password, Salt, WorkFactor);
        }

        /// <summary>
        /// Use this when verifying an existing user
        /// </summary>
        /// <param name="password">The password entered by the user</param>
        /// <param name="salt">The salt used to create the original hash</param>
        public PasswordManager(string password, string salt)
        {
            Salt = salt;
            Hash = GenerateHash(password, salt, WorkFactor);
        }

        /// <summary>
        /// Verifies if the passed in hash matches the stored hash property
        /// </summary>
        /// <param name="hash">The hash to be verified</param>
        /// <returns>True if the passed in hash and the stored hash are the same</returns>
        public bool Verify(string hash)
        {
            return Hash == hash;
        }

        /// <summary>
        /// Generates a random salt value
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>Salt string</returns>
        private static string GenerateSalt(string password, int saltSize, int workFactor)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltSize, workFactor);
            return Convert.ToBase64String(rfc.Salt);
        }

        /// <summary>
        /// Generates the hash for the
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hash for the passed in password, salt, and work factor</returns>
        private static string GenerateHash(string password, string salt, int workFactor)
        {
            Rfc2898DeriveBytes rfc = HashPasswordWithPBKDF2(password, salt, workFactor);
            return Convert.ToBase64String(rfc.GetBytes(20));
        }

        /// <summary>
        /// Generates the RFC object given the password, salt, and work factor
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <param name="salt">The salt to use in the hashing process</param>
        /// <param name="workFactor">The work factor needed for the hashing process</param>
        /// <returns>RFC object</returns>
        private static Rfc2898DeriveBytes HashPasswordWithPBKDF2(string password, string salt, int workFactor)
        {
            // Creates the crypto service provider and provides the salt - usually used to check for a password match
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), workFactor);

            return rfc2898DeriveBytes;
        }

        /// <summary>
        /// Decodes the passed in RSA encrypted text given the private key path
        /// </summary>
        /// <param name="cipherText">RSA encrypted text</param>
        /// <param name="privateKeyPath">The path to the private key xml file</param>
        /// <returns>The decrypted text</returns>
        public static string DecryptCipherText(string cipherText, string privateKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(privateKeyPath));
            byte[] original = cipher.Decrypt(Convert.FromBase64String(cipherText), false);
            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// RSA encrypts the passed in text given the public key path
        /// </summary>
        /// <param name="plaintext">The text to be encrypted</param>
        /// <param name="publicKeyPath">The path to the public key xml file</param>
        /// <returns>The RSA encrypted text</returns>
        public static string GetCipherText(string plaintext, string publicKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(publicKeyPath));
            byte[] data = Encoding.UTF8.GetBytes(plaintext);
            byte[] cipherText = cipher.Encrypt(data, false);
            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Generates a set of public/private key xml files for use in RSA encryption
        /// </summary>
        /// <param name="publicKeyFileName">The full filepath to the public key file to be created</param>
        /// <param name="privateKeyFileName">The full filepath to the private key file to be created</param>
        public static void GenerateKeys(string publicKeyFileName, string privateKeyFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamWriter publicKeyFile = null;
            StreamWriter privateKeyFile = null;

            string publicKey = "";
            string privateKey = "";

            try
            {
                // Create a new key pair on target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL
                //cspParams.ProviderName; // CSP name

                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Export public key
                publicKey = rsaProvider.ToXmlString(false);

                // Write public key to file 
                publicKeyFile = System.IO.File.CreateText(publicKeyFileName);
                publicKeyFile.Write(publicKey);

                // Export private/public key pair
                privateKey = rsaProvider.ToXmlString(true);

                // Write private/public key pair to file
                privateKeyFile = System.IO.File.CreateText(privateKeyFileName);
                privateKeyFile.Write(privateKey);
            }
            finally
            {
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }

                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
            }
        }
        
    }
}
