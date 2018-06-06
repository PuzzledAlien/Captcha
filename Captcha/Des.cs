using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Captcha
{
    /// <summary>
    /// DES加密解密
    /// </summary>
    public class Des
    {
        /// <summary>
        /// 加密密钥
        /// </summary>
        private const string SecretKey = "LOVEROSE";
        /// <summary>
        /// 默认UTF8编码
        /// </summary>
        private static readonly Encoding Default = Encoding.UTF8;

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<string> EncryptAsync(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            using (var p = new DESCryptoServiceProvider())
            {
                p.IV = Default.GetBytes(SecretKey);
                p.Key = Default.GetBytes(SecretKey);
                using (var ct = p.CreateEncryptor(p.IV, p.Key))
                {
                    var temp = Default.GetBytes(val);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(temp, 0, temp.Length);
                            await cs.FlushAsync();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static async Task<string> DecryptAsync(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            using (var p = new DESCryptoServiceProvider())
            {
                p.IV = Default.GetBytes(SecretKey);
                p.Key = Default.GetBytes(SecretKey);
                using (var ct = p.CreateDecryptor(p.IV, p.Key))
                {
                    var temp = Convert.FromBase64String(val);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(temp, 0, temp.Length);
                            await cs.FlushAsync();
                        }
                        return Default.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
