using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Natures.Commands;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Natures.Queries;
using SkillCraft.Tools.Models.Nature;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/natures")]
public class NatureController : ControllerBase
{
  private readonly IMediator _mediator;

  public NatureController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<NatureModel>> CreateAsync([FromBody] CreateOrReplaceNaturePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceNatureCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceNatureResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<NatureModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadNatureQuery query = new(id, Slug: null);
    NatureModel? nature = await _mediator.Send(query, cancellationToken);
    return nature == null ? NotFound() : Ok(nature);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<NatureModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadNatureQuery query = new(Id: null, slug);
    NatureModel? nature = await _mediator.Send(query, cancellationToken);
    return nature == null ? NotFound() : Ok(nature);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<NatureModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceNaturePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceNatureCommand command = new(id, payload, version);
    CreateOrReplaceNatureResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<NatureModel>>> SearchAsync([FromQuery] SearchNaturesParameters parameters, CancellationToken cancellationToken)
  {
    SearchNaturesQuery query = new(parameters.ToPayload());
    SearchResults<NatureModel> natures = await _mediator.Send(query, cancellationToken);
    return Ok(natures);
  }

  private ActionResult<NatureModel> ToActionResult(CreateOrReplaceNatureResult result)
  {
    NatureModel? nature = result.Nature;
    if (nature == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(nature);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/natures/{nature.Id}", UriKind.Absolute);
    return Created(uri, nature);
  }
}
