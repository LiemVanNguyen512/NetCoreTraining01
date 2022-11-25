using Microsoft.AspNetCore.Mvc;

namespace Course_service.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
