using Microsoft.AspNetCore.Mvc;

namespace SkillCraft.Tools.Controllers;

[Route("")]
public class IndexController : Controller
{
  [HttpGet]
  public ActionResult Index()
  {
    return View();
  }
}
