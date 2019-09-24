using System.Threading.Tasks;
using CaptchaService;
using CaptchaService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Captcha.WebApi.Controllers
{
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaFactory _captchaFactory;
        public CaptchaController(ICaptchaFactory captchaFactory)
        {
            _captchaFactory = captchaFactory;
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/captcha")]
        public async Task<CaptchaInfo> GetCaptcha()
        {
            var model = await _captchaFactory.CreateAsync();
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
            var response = await _captchaFactory.VerifyAsync(model);
            return response;
        }
    }
}
