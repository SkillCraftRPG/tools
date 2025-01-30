using Logitar.Cms.Core.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Natures.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("natures")]
public class NatureController : Controller
{
  private readonly IMediator _mediator;

  public NatureController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> NatureList(CancellationToken cancellationToken)
  {
    SearchNaturesPayload payload = new();
    payload.Sort.Add(new NatureSortOption(NatureSort.DisplayName));
    SearchNaturesQuery query = new(payload);
    SearchResults<NatureModel> natures = await _mediator.Send(query, cancellationToken);

    return View(natures);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> NatureView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadNatureQuery query = new(parsed ? id : null, idOrSlug);
    NatureModel? nature = await _mediator.Send(query, cancellationToken);
    if (nature == null)
    {
      return NotFound();
    }

    return View(nature);
  }
}
