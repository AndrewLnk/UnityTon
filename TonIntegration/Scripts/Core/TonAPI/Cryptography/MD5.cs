using System;
using System.Security.Cryptography;
using System.Text;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Cryptography
{
    public static class Md5
    {
        public static string Md5Encryption(string inputData, string key)
        {
            byte[] bData = Encoding.UTF8.GetBytes(inputData);
 
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripalDES = new TripleDESCryptoServiceProvider();
 
            tripalDES.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            tripalDES.Mode = CipherMode.ECB;
 
            ICryptoTransform trnsfrm = tripalDES.CreateEncryptor();
            byte[] result = trnsfrm.TransformFinalBlock(bData, 0, bData.Length);
            
            return Convert.ToBase64String(result);
        }
        
        public static string Md5Decryption(string inputData, string key)
        {
            byte[] bData = Convert.FromBase64String(inputData);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripalDES = new TripleDESCryptoServiceProvider();
 
            tripalDES.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            tripalDES.Mode = CipherMode.ECB;
 
            ICryptoTransform trnsfrm = tripalDES.CreateDecryptor();
            
            try
            {
                byte[] result = trnsfrm.TransformFinalBlock(bData, 0, bData.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
