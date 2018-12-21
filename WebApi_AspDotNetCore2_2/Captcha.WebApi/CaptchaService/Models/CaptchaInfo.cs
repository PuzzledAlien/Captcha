namespace CaptchaService.Models
{
    public class CaptchaInfo
    {
        /// <summary>
        /// 图片字节
        /// </summary>
        public byte[] Image { get; set; }
        /// <summary>
        /// 图片中显示的字符（经过DES加密）
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 设计后续有类似问题类的图片验证码
        /// </summary>
        public string Tip { get; set; }
        /// <summary>
        /// 验证码图片文件为类型
        /// </summary>
        public string ContentType { get; set; }
    }
}
