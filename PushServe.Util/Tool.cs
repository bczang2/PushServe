using System;
using System.Security.Cryptography;
using System.Text;

namespace PushServe.Util
{
    public class Tool
    {
        public static string MD5(int value)
        {
            return MD5(value.ToString());
        }

        public static string MD5(long value)
        {
            return MD5(value.ToString());
        }

        public static string MD5(string value)
        {
            HashAlgorithm HA = new MD5CryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(value);
            byte[] encodedBytes = HA.ComputeHash(inputByteArray);

            return (BitConverter.ToString(encodedBytes)).Replace("-", "").ToLower();
        }

        public static bool MD5Check(string plaintext, string md5Token)
        {
            return MD5(plaintext + Constant.TokenKeyt).Equals(md5Token);
        }
    }
}
