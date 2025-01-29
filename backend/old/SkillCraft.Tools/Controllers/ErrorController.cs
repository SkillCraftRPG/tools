using Microsoft.AspNetCore.Mvc;

namespace SkillCraft.Tools.Controllers;

public class ErrorController : Controller
{
  [HttpGet("not-found")]
  public ActionResult NotFound404() => View();
}
