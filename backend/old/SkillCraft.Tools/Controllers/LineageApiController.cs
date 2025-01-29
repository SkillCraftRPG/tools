using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Lineages.Commands;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Core.Lineages.Queries;
using SkillCraft.Tools.Models.Lineage;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/lineages")]
public class LineageApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public LineageApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<LineageModel>> CreateAsync([FromBody] CreateOrReplaceLineagePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceLineageCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceLineageResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LineageModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadLineageQuery query = new(id, Slug: null);
    LineageModel? lineage = await _mediator.Send(query, cancellationToken);
    return lineage == null ? NotFound() : Ok(lineage);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<LineageModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadLineageQuery query = new(Id: null, slug);
    LineageModel? lineage = await _mediator.Send(query, cancellationToken);
    return lineage == null ? NotFound() : Ok(lineage);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<LineageModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceLineagePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceLineageCommand command = new(id, payload, version);
    CreateOrReplaceLineageResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<LineageModel>>> SearchAsync([FromQuery] SearchLineagesParameters parameters, CancellationToken cancellationToken)
  {
    SearchLineagesQuery query = new(parameters.ToPayload());
    SearchResults<LineageModel> lineages = await _mediator.Send(query, cancellationToken);
    return Ok(lineages);
  }

  private ActionResult<LineageModel> ToActionResult(CreateOrReplaceLineageResult result)
  {
    LineageModel? lineage = result.Lineage;
    if (lineage == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(lineage);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/lineages/{lineage.Id}", UriKind.Absolute);
    return Created(uri, lineage);
  }
}
