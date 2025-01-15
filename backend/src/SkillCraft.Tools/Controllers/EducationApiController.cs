using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Educations.Commands;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Educations.Queries;
using SkillCraft.Tools.Models.Education;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/educations")]
public class EducationApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public EducationApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<EducationModel>> CreateAsync([FromBody] CreateOrReplaceEducationPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceEducationCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceEducationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<EducationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadEducationQuery query = new(id, Slug: null);
    EducationModel? education = await _mediator.Send(query, cancellationToken);
    return education == null ? NotFound() : Ok(education);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<EducationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadEducationQuery query = new(Id: null, slug);
    EducationModel? education = await _mediator.Send(query, cancellationToken);
    return education == null ? NotFound() : Ok(education);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<EducationModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceEducationPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceEducationCommand command = new(id, payload, version);
    CreateOrReplaceEducationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<EducationModel>>> SearchAsync([FromQuery] SearchEducationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchEducationsQuery query = new(parameters.ToPayload());
    SearchResults<EducationModel> educations = await _mediator.Send(query, cancellationToken);
    return Ok(educations);
  }

  private ActionResult<EducationModel> ToActionResult(CreateOrReplaceEducationResult result)
  {
    EducationModel? education = result.Education;
    if (education == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(education);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/educations/{education.Id}", UriKind.Absolute);
    return Created(uri, education);
  }
}
