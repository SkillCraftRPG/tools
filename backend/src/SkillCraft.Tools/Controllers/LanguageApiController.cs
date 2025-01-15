using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Languages.Commands;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Languages.Queries;
using SkillCraft.Tools.Models.Language;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/languages")]
public class LanguageApiController : ControllerBase
{
  private readonly IMediator _mediator;

  public LanguageApiController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<LanguageModel>> CreateAsync([FromBody] CreateOrReplaceLanguagePayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceLanguageCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceLanguageResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<LanguageModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadLanguageQuery query = new(id, Slug: null);
    LanguageModel? language = await _mediator.Send(query, cancellationToken);
    return language == null ? NotFound() : Ok(language);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<LanguageModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadLanguageQuery query = new(Id: null, slug);
    LanguageModel? language = await _mediator.Send(query, cancellationToken);
    return language == null ? NotFound() : Ok(language);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<LanguageModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceLanguagePayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceLanguageCommand command = new(id, payload, version);
    CreateOrReplaceLanguageResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<LanguageModel>>> SearchAsync([FromQuery] SearchLanguagesParameters parameters, CancellationToken cancellationToken)
  {
    SearchLanguagesQuery query = new(parameters.ToPayload());
    SearchResults<LanguageModel> languages = await _mediator.Send(query, cancellationToken);
    return Ok(languages);
  }

  private ActionResult<LanguageModel> ToActionResult(CreateOrReplaceLanguageResult result)
  {
    LanguageModel? language = result.Language;
    if (language == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(language);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/languages/{language.Id}", UriKind.Absolute);
    return Created(uri, language);
  }
}
