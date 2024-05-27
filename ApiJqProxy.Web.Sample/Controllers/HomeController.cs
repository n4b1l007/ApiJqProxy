using Microsoft.AspNetCore.Mvc;

namespace ApiJqProxy.Web.Sample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
