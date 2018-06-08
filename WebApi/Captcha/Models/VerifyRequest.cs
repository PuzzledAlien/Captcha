namespace Captcha.Models
{
    public class VerifyRequest
    {
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// Cookie中对应Captcha的值
        /// </summary>
        public string Captcha { get; set; }
    }
}
