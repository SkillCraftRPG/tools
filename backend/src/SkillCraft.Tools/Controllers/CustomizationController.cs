using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Constants;
using SkillCraft.Tools.Core.Customizations.Commands;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Customizations.Queries;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Models.Customization;

namespace SkillCraft.Tools.Controllers;

[ApiController]
[Authorize(Policy = Policies.IsAdmin)]
[Route("api/customizations")]
public class CustomizationController : ControllerBase
{
  private readonly IMediator _mediator;

  public CustomizationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost]
  public async Task<ActionResult<CustomizationModel>> CreateAsync([FromBody] CreateOrReplaceCustomizationPayload payload, CancellationToken cancellationToken)
  {
    CreateOrReplaceCustomizationCommand command = new(Id: null, payload, Version: null);
    CreateOrReplaceCustomizationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<CustomizationModel>> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    ReadCustomizationQuery query = new(id, Slug: null);
    CustomizationModel? customization = await _mediator.Send(query, cancellationToken);
    return customization == null ? NotFound() : Ok(customization);
  }

  [HttpGet("slug:{slug}")]
  public async Task<ActionResult<CustomizationModel>> ReadAsync(string slug, CancellationToken cancellationToken)
  {
    ReadCustomizationQuery query = new(Id: null, slug);
    CustomizationModel? customization = await _mediator.Send(query, cancellationToken);
    return customization == null ? NotFound() : Ok(customization);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<CustomizationModel>> ReplaceAsync(Guid id, [FromBody] CreateOrReplaceCustomizationPayload payload, long? version, CancellationToken cancellationToken)
  {
    CreateOrReplaceCustomizationCommand command = new(id, payload, version);
    CreateOrReplaceCustomizationResult result = await _mediator.Send(command, cancellationToken);
    return ToActionResult(result);
  }

  [HttpGet]
  public async Task<ActionResult<SearchResults<CustomizationModel>>> SearchAsync([FromQuery] SearchCustomizationsParameters parameters, CancellationToken cancellationToken)
  {
    SearchCustomizationsQuery query = new(parameters.ToPayload());
    SearchResults<CustomizationModel> customizations = await _mediator.Send(query, cancellationToken);
    return Ok(customizations);
  }

  private ActionResult<CustomizationModel> ToActionResult(CreateOrReplaceCustomizationResult result)
  {
    CustomizationModel? customization = result.Customization;
    if (customization == null)
    {
      return NotFound();
    }
    else if (!result.Created)
    {
      return Ok(customization);
    }

    Uri uri = new($"{Request.Scheme}://{Request.Host}/api/customizations/{customization.Id}", UriKind.Absolute);
    return Created(uri, customization);
  }
}
