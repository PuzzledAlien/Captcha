using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Captcha.Models;

namespace Captcha
{
    public partial class CaptchaFactory
    {
        /// <summary>
        /// 绘制干扰线
        /// </summary>
        /// <param name="random"></param>
        /// <param name="bitmap"></param>
        /// <param name="graphics"></param>
        /// <param name="lineCount"></param>
        /// <param name="pointCount"></param>
        public static void Disturb(Random random, Bitmap bitmap, Graphics graphics, int lineCount, int pointCount)
        {

            var colors = new List<Color>
            {
                Color.AliceBlue,
                Color.Azure,
                Color.CadetBlue,
                Color.Beige,
                Color.Chartreuse
            };

            //干扰线
            for (var i = 0; i < lineCount; i++)
            {
                var x1 = random.Next(bitmap.Width);
                var x2 = random.Next(bitmap.Width);
                var y1 = random.Next(bitmap.Height);
                var y2 = random.Next(bitmap.Height);

                //Pen 类 定义用于绘制直线和曲线的对象。
                var pen = new Pen(colors[random.Next(0, colors.Count - 1)]);

                graphics.DrawLine(pen, x1, y1, x2, y2);
            }

            //干扰点
            for (var i = 0; i < pointCount; i++)
            {
                var x = random.Next(bitmap.Width);
                var y = random.Next(bitmap.Height);
                bitmap.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
        }
    }

    public partial class CaptchaFactory
    {
        public static CaptchaFactory Intance = new CaptchaFactory();

        private static List<char> _characters;

        private const string ContentType = "image/jpeg";

        public CaptchaFactory()
        {
            _characters = new List<char>();
            //去掉0、o、O
            for (var c = '0'; c <= '9'; c++)
            {
                if (c == '0')
                {
                    continue;
                }
                _characters.Add(c);
            }
            for (var c = 'a'; c < 'z'; c++)
            {
                if (c == 'o')
                {
                    continue;
                }
                _characters.Add(c);
            }
            for (var c = 'A'; c < 'Z'; c++)
            {
                if (c == 'O')
                {
                    continue;
                }
                _characters.Add(c);
            }
        }

        public async Task<CaptchaInfo> CreateAsync(int charCount = 4, int width = 85, int height = 40)
        {
            var model = new CaptchaInfo
            {
                ContentType = ContentType
            };

            var chars = new char[charCount];
            var len = _characters.Count;
            var random = new Random();
            for (var i = 0; i < chars.Length; i++)
            {
                var r = random.Next(len);
                chars[i] = _characters[r];
            }

            var answer = string.Join(string.Empty, chars);

            model.Answer = await Des.EncryptAsync($"{answer}|{DateTime.Now}");

            //var fontNames = FontFamily.Families.Select(_ => _.Name).ToList();
            var fontNames = new List<string>
            {
                "Helvetica","Arial","Lucida Family","Verdana","Tahoma","Trebuchet MS","Georgia","Times"
            };

            //Bitmap 类 封装 GDI+ 包含图形图像和其属性的像素数据的位图。 一个 Bitmap 是用来处理图像像素数据所定义的对象。
            //Bitmap 类 继承自 抽象基类 Image 类
            using (var bitmap = new Bitmap(width, height))
            {
                //Graphics 类 封装一个 GDI+ 绘图图面。
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    //填充背景色 白色
                    graphics.Clear(Color.White);

                    //绘制干扰线和干扰点
                    Disturb(random, bitmap, graphics, width / 2, height);

                    //添加灰色边框
                    var pen = new Pen(Color.Silver);
                    graphics.DrawRectangle(pen, 0, 0, width - 1, height - 1);

                    var x = 1;
                    const int y = 5;

                    var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

                    var color = Color.FromArgb(random.Next(100, 122), random.Next(100, 122), random.Next(100, 122));

                    foreach (var c in chars)
                    {
                        //随机选择字符 字体样式和大小
                        var fontName = fontNames[random.Next(0, fontNames.Count - 1)];
                        var font = new Font(fontName, random.Next(15, 20));
                        //淡化字符颜色 
                        using (var brush = new LinearGradientBrush(rectangle, color, color, 90f, true))
                        {
                            brush.SetSigmaBellShape(0.5f);
                            graphics.DrawString(c.ToString(), font, brush, x + random.Next(-2, 2), y + random.Next(-5, 5));
                            x = x + width / charCount;
                        }
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        bitmap.Save(memoryStream, ImageFormat.Jpeg);

                        model.Image = memoryStream.ToArray();

                        return model;
                    }
                }
            }
        }
    }

    public partial class CaptchaFactory
    {
        /// <summary>
        /// 使用线程安全字典，实现验证码只能验证一次的功能，防止机器暴力破解验证码
        /// </summary>
        private static readonly ConcurrentDictionary<string, DateTime> Dic = new ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// 比对验证码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeOut">单位秒，超时时间默认600秒（5分钟）</param>
        /// <returns></returns>
        public async Task<VerifyResponse> VerifyAsync(VerifyRequest model, int timeOut = 600)
        {
            //判空
            if (string.IsNullOrEmpty(model.Answer) || string.IsNullOrEmpty(model.Captcha))
            {
                return new VerifyResponse
                {
                    Code = 102,
                    Message = "验证失败"
                };
            }

            //一个验证码只能调用一次接口
            if (Dic.ContainsKey(model.Captcha))
            {
                return new VerifyResponse
                {
                    Code = 101,
                    Message = "验证码失效"
                };
            }

            //记录第一次调用
            Dic.TryAdd(model.Captcha, DateTime.Now);

            //清除垃圾数据
            foreach (var d in Dic)
            {
                var day = (d.Value - DateTime.Now).Days;
                if (day > 1)
                {
                    Dic.TryRemove(d.Key, out var date);
                }
            }

            var captcha = await Des.DecryptAsync(model.Captcha);
            var temp = captcha.Split('|');
            if (!DateTime.TryParse(temp[1], out var dateTime))
            {
                return new VerifyResponse
                {
                    Code = 101,
                    Message = "验证码失效"
                };
            }

            var sec = (DateTime.Now - dateTime).TotalSeconds;

            if (sec > timeOut)
            {
                return new VerifyResponse
                {
                    Code = 101,
                    Message = "验证码失效"
                };
            }

            var answer = temp[0];
            if (string.Equals(answer, model.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                return new VerifyResponse
                {
                    Code = 100,
                    Message = "验证成功"
                };
            }

            return new VerifyResponse
            {
                Code = 102,
                Message = "验证失败"
            };
        }
    }
}
