using Microsoft.AspNetCore.Mvc;

namespace Secretary.Controllers
{
    public class SecretaryController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}
