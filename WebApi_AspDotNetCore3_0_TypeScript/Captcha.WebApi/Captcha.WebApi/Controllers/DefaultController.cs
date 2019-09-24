using System.Threading.Tasks;
using CaptchaService;
using Microsoft.AspNetCore.Mvc;

namespace Captcha.WebApi.Controllers
{
    public class DefaultController : Controller
    {
        private readonly ICaptchaFactory _captchaFactory;
        public DefaultController(ICaptchaFactory captchaFactory)
        {
            _captchaFactory = captchaFactory;
        }

        [HttpGet, Route("index")]
        public IActionResult Index()
        {
            return View("/Views/Index.cshtml");
        }

        /// <summary>
        /// 返回验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("captcha")]
        public async Task<ActionResult> GetCaptcha()
        {
            var model = await _captchaFactory.CreateAsync();
            Response.Cookies.Append("Captcha", model.Answer);
            return File(model.Image, model.ContentType);
        }
    }
}