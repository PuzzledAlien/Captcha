//=====================================================

//Copyright (C) 2016-2018 Fanjia

//All rights reserved

//CLR版 本:    4.0.30319.42000

//创建时间:     2018/12/21 17:41:02

//创 建 人:   徐晓航

//======================================================

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CaptchaService.Extensions
{
    /// <summary>
    /// DES加密解密
    /// </summary>
    public static class DesExtension
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
        public static async Task<string> EncryptAsync(this string val)
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
                            cs.Close();
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
        public static async Task<string> DecryptAsync(this string val)
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
                    val = WebUtility.UrlDecode(val);
                    val = WebUtility.UrlDecode(val);
                    var temp = Convert.FromBase64String(val);
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
                        {
                            await cs.WriteAsync(temp, 0, temp.Length);
                            await cs.FlushAsync();
                            cs.Close();
                        }
                        return Default.GetString(ms.ToArray());
                    }
                }
            }
        }
    }
}
