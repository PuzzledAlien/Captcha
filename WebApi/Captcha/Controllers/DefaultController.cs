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

        [HttpGet, Route("captcha")]
        public async Task<ActionResult> GetCaptcha()
        {
            var model = await CaptchaFactory.Intance.CreateAsync();
            Response.Cookies.Append("Captcha",model.Answer);
            return File(model.Image, model.ContentType);
        }
    }
}