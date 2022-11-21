using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
