using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Models.Index;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Route("api")]
public class IndexApiController : ControllerBase
{
  [HttpGet]
  public ActionResult<ApiVersion> Get() => Ok(ApiVersion.Current);
}
