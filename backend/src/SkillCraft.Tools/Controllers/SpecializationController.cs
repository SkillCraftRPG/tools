using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Specializations.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("specialisations")]
public class SpecializationController : Controller
{
  private readonly IMediator _mediator;

  public SpecializationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> SpecializationList(CancellationToken cancellationToken)
  {
    SearchSpecializationsPayload payload = new();
    SearchSpecializationsQuery query = new(payload);
    SearchResults<SpecializationModel> specializations = await _mediator.Send(query, cancellationToken);

    return View(specializations);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> SpecializationView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadSpecializationQuery query = new(parsed ? id : null, idOrSlug);
    SpecializationModel? specialization = await _mediator.Send(query, cancellationToken);
    if (specialization == null)
    {
      return NotFound();
    }

    return View(specialization);
  }
}
