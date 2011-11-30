using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace UMR.Saniteri.DataFactory
{
    [DataContract]
    public class DatabaseSettings
    {
        [DataMember]
        public string databaseTechnology { get; set; }
        [DataMember]
        public string serverName { get; set; }
        [DataMember]
        public bool integratedSecurity { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string databaseVersion { get; set; }

        public string password
        {
            get { return decrypt(this.passwordEncrypt); }
            set { this.passwordEncrypt = encrypt(value); }
        }

        [DataMember]
        public string passwordEncrypt { get; set; }

        private static String encrypt(string source)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var des = new DESCryptoServiceProvider();
            byte[] Key = { 12, 13, 14, 15, 16, 17, 18, 19 };
            byte[] IV = { 12, 13, 14, 15, 16, 17, 18, 19 };
            var encryptor = des.CreateEncryptor(Key, IV);
            byte[] IDToBytes = ASCIIEncoding.ASCII.GetBytes(source);
            byte[] encryptedID = encryptor.TransformFinalBlock(IDToBytes, 0, IDToBytes.Length);
            return Convert.ToBase64String(encryptedID);
        }

        private static string decrypt(string encrypted)
        {
            if (string.IsNullOrEmpty(encrypted)) return null;
            byte[] Key = { 12, 13, 14, 15, 16, 17, 18, 19 };
            byte[] IV = { 12, 13, 14, 15, 16, 17, 18, 19 };
            var des = new DESCryptoServiceProvider();
            var decryptor = des.CreateDecryptor(Key, IV);
            byte[] encryptedIDToBytes = Convert.FromBase64String(encrypted);
            byte[] IDToBytes = decryptor.TransformFinalBlock(encryptedIDToBytes, 0, encryptedIDToBytes.Length);
            return ASCIIEncoding.ASCII.GetString(IDToBytes);
        }
    }
}
