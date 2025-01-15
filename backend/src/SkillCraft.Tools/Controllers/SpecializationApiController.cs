using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Specializations.Commands;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Specializations.Queries;
using SkillCraft.Tools.Models.Specialization;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/specializations")]
public class SpecializationApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public SpecializationApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<SpecializationModel>> CreateAsync([FromBody] CreateOrReplaceSpecializationPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpecializationCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceSpecializationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
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

  [HttpPut("{id}")]
  public async Task<ActionResult<SpecializationModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceSpecializationPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpecializationCommand command = new(id, payload, version);
    CreateOrReplaceSpecializationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<SpecializationModel>>> SearchAsync([FromQuery] SearchSpecializationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchSpecializationsQuery query = new(parameters.ToPayload());
    SearchResults<SpecializationModel> specializations = await _mediator.Send(query, cancellationToken);
    return Ok(specializations);
  }

  private ActionResult<SpecializationModel> ToActionResult(CreateOrReplaceSpecializationResult result)
  {
    SpecializationModel? specialization = result.Specialization;
    if (specialization == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(specialization);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/specializations/{specialization.Id}", UriKind.Absolute);
    return Created(uri, specialization);
  }
}
