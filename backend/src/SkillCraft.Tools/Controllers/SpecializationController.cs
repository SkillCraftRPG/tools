using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Specializations.Queries;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/specializations")]
public class SpecializationController : ControllerBase
{
  private readonly IMediator _mediator;

  public SpecializationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<SpecializationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadSpecializationQuery query = new(id, Slug: null);
    SpecializationModel? specialization = await _mediator.Send(query, cancellationToken);
    return specialization == null ? NotFound() : Ok(specialization);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<SpecializationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadSpecializationQuery query = new(Id: null, slug);
    SpecializationModel? specialization = await _mediator.Send(query, cancellationToken);
    return specialization == null ? NotFound() : Ok(specialization);
  }
}
