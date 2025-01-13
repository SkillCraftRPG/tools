using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Aspects.Commands;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Aspects.Queries;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Models.Aspect;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/aspects")]
public class AspectController : ControllerBase
{
  private readonly IMediator _mediator;

  public AspectController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<AspectModel>> CreateAsync([FromBody] CreateOrReplaceAspectPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceAspectCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceAspectResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<AspectModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadAspectQuery query = new(id, Slug: null);
    AspectModel? aspect = await _mediator.Send(query, cancellationToken);
    return aspect == null ? NotFound() : Ok(aspect);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<AspectModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadAspectQuery query = new(Id: null, slug);
    AspectModel? aspect = await _mediator.Send(query, cancellationToken);
    return aspect == null ? NotFound() : Ok(aspect);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<AspectModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceAspectPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceAspectCommand command = new(id, payload, version);
    CreateOrReplaceAspectResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<AspectModel>>> SearchAsync([FromQuery] SearchAspectsParameters parameters, CancellationToken cancellationToken)
  {
    SearchAspectsQuery query = new(parameters.ToPayload());
    SearchResults<AspectModel> aspects = await _mediator.Send(query, cancellationToken);
    return Ok(aspects);
  }

  private ActionResult<AspectModel> ToActionResult(CreateOrReplaceAspectResult result)
  {
    AspectModel? aspect = result.Aspect;
    if (aspect == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(aspect);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/aspects/{aspect.Id}", UriKind.Absolute);
    return Created(uri, aspect);
  }
}
