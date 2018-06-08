using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Captcha.Controllers
{
    public class DefaultController : Controller
    {
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
            var model = await CaptchaFactory.Intance.CreateAsync();
            Response.Cookies.Append("Captcha", model.Answer);
            return File(model.Image, model.ContentType);
        }
    }
}