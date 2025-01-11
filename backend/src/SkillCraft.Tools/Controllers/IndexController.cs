using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Models.Index;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Route("")]
public class IndexController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(ApiVersion.Current);
}
