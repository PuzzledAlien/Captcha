using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Captcha.Models;

namespace Captcha.Controllers
{
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("api/captcha")]
        public async Task<CaptchaInfo> GetCaptcha()
        {
            var model = await CaptchaFactory.Intance.CreateAsync();
            return model;
        }

        /// <summary>
        /// 验证码的校验
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("api/captcha/verify")]
        public async Task<VerifyResponse> Verify([FromBody] VerifyRequest model)
        {
            var response = await CaptchaFactory.Intance.VerifyAsync(model);

            return response;
        }
    }
}
