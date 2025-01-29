using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Castes.Commands;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Castes.Queries;
using SkillCraft.Tools.Models.Caste;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/castes")]
public class CasteApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public CasteApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<CasteModel>> CreateAsync([FromBody] CreateOrReplaceCastePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceCasteCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceCasteResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CasteModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadCasteQuery query = new(id, Slug: null);
    CasteModel? caste = await _mediator.Send(query, cancellationToken);
    return caste == null ? NotFound() : Ok(caste);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<CasteModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadCasteQuery query = new(Id: null, slug);
    CasteModel? caste = await _mediator.Send(query, cancellationToken);
    return caste == null ? NotFound() : Ok(caste);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<CasteModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceCastePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceCasteCommand command = new(id, payload, version);
    CreateOrReplaceCasteResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<CasteModel>>> SearchAsync([FromQuery] SearchCastesParameters parameters, CancellationToken cancellationToken)
  {
    SearchCastesQuery query = new(parameters.ToPayload());
    SearchResults<CasteModel> castes = await _mediator.Send(query, cancellationToken);
    return Ok(castes);
  }

  private ActionResult<CasteModel> ToActionResult(CreateOrReplaceCasteResult result)
  {
    CasteModel? caste = result.Caste;
    if (caste == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(caste);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/castes/{caste.Id}", UriKind.Absolute);
    return Created(uri, caste);
  }
}
