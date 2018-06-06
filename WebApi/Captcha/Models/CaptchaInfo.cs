namespace Captcha.Models
{
    public class CaptchaInfo
    {
        public byte[] Image { get; set; }
        public string Answer { get; set; }
        public string Tip { get; set; }
        public string ContentType { get; set; }
    }
}
