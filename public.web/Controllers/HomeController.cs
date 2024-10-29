using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("~/")]
public class HomeController() : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
