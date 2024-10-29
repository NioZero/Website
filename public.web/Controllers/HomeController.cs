using Microsoft.AspNetCore.Mvc;

namespace NioZero.Web.Controllers;

[Route("~/")]
public class HomeController() : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
