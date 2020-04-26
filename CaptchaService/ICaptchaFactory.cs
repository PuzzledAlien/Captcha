using System.Threading.Tasks;
using CaptchaService.Models;

namespace CaptchaService
{
    public interface ICaptchaFactory
    {
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="charCount">图片中出现字符数</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <returns></returns>
        Task<CaptchaInfo> CreateAsync(int charCount = 4, int width = 85, int height = 40);
        /// <summary>
        /// 比对验证码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeOut">单位秒，超时时间默认600秒（5分钟）</param>
        /// <returns></returns>
        Task<VerifyResponse> VerifyAsync(VerifyRequest model, int timeOut = 600);
    }
}
