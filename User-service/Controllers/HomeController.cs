using Microsoft.AspNetCore.Mvc;

namespace User_service.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
