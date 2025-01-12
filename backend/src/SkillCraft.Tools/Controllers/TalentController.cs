using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Core.Talents.Commands;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Queries;
using SkillCraft.Tools.Models.Talent;

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

  [HttpPost]
  public async Task<ActionResult<TalentModel>> CreateAsync([FromBody] CreateOrReplaceTalentPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceTalentCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceTalentResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
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

  [HttpPut("{id}")]
  public async Task<ActionResult<TalentModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceTalentPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceTalentCommand command = new(id, payload, version);
    CreateOrReplaceTalentResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<TalentModel>>> SearchAsync([FromQuery] SearchTalentsParameters parameters, CancellationToken cancellationToken)
  {
    SearchTalentsQuery query = new(parameters.ToPayload());
    SearchResults<TalentModel> talents = await _mediator.Send(query, cancellationToken);
    return Ok(talents);
  }

  private ActionResult<TalentModel> ToActionResult(CreateOrReplaceTalentResult result)
  {
    TalentModel? talent = result.Talent;
    if (talent == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(talent);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/talents/{talent.Id}", UriKind.Absolute);
    return Created(uri, talent);
  }
}
