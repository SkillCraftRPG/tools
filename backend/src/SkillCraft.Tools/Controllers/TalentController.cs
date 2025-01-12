using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Queries;

namespace SkillCraft.Tools.Controllers;

[ApiController]
//[Authorize] // ISSUE: https://github.com/SkillCraftRPG/tools/issues/5
[Route("api/talents")]
public class TalentController : ControllerBase
{
  private readonly IMediator _mediator;

  public TalentController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<TalentModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadTalentQuery query = new(id, Slug: null);
    TalentModel? talent = await _mediator.Send(query, cancellationToken);
    return talent == null ? NotFound() : Ok(talent);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<TalentModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadTalentQuery query = new(Id: null, slug);
    TalentModel? talent = await _mediator.Send(query, cancellationToken);
    return talent == null ? NotFound() : Ok(talent);
  }
}
