using Microsoft.AspNetCore.Mvc;

namespace XProject.WebApi.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
