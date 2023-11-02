using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Common
{
    public class AppCommon
    {
        

        public string PathCombine(params string[] Paths)
        {
            if (Paths.Length <= 0) { return string.Empty; }
            string firstPath = Paths[0];

            string filePath = string.Empty;
            bool isFirstPath = true;
            foreach (string nextPath in Paths)
            {
                if (isFirstPath) { isFirstPath = false; continue; }
                filePath = Path.Combine(filePath, nextPath);
            }

            return firstPath.TrimEnd(new char[] { '\\' }) + @"\" + filePath.TrimStart(new char[] { '\\' });
        }

        public string Encrypt(string clearText, string EncryptionKey = "")
        {
            if (string.IsNullOrEmpty(EncryptionKey))
            {
                EncryptionKey = MyApp.Setting.EncryptionKey;
            }
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public string Decrypt(string cipherText, string EncryptionKey = "")
        {
            if (string.IsNullOrEmpty(EncryptionKey))
            {
                EncryptionKey = MyApp.Setting.EncryptionKey;
            }

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}