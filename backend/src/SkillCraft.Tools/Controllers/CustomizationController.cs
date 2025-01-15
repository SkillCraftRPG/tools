using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Core.Customizations.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("dons-handicaps")]
public class CustomizationController : Controller
{
  private readonly IMediator _mediator;

  public CustomizationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> CustomizationList(CancellationToken cancellationToken)
  {
    SearchCustomizationsPayload payload = new();
    SearchCustomizationsQuery query = new(payload);
    SearchResults<CustomizationModel> customizations = await _mediator.Send(query, cancellationToken);

    return View(customizations);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> CustomizationView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadCustomizationQuery query = new(parsed ? id : null, idOrSlug);
    CustomizationModel? customization = await _mediator.Send(query, cancellationToken);
    if (customization == null)
    {
      return NotFound();
    }

    return View(customization);
  }
}
