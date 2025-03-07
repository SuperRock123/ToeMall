using System;
using System.Security.Cryptography;
using System.Text;

namespace ToeMall.Utils
{
    public class HashEncoding
    {
        //使用 SHA-256 生成哈希值
        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be null or empty");

            using (SHA256 sha256 = SHA256.Create())
            {
                // 这里应该使用 Encoding.UTF8，而不是 HashEncoding.UTF8
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // 将哈希结果转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
