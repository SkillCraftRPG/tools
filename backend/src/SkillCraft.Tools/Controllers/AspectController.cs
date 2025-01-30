using Logitar.Cms.Core.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Aspects.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("aspects")]
public class AspectController : Controller
{
  private readonly IMediator _mediator;

  public AspectController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> AspectList(CancellationToken cancellationToken)
  {
    SearchAspectsPayload payload = new();
    payload.Sort.Add(new AspectSortOption(AspectSort.DisplayName));
    SearchAspectsQuery query = new(payload);
    SearchResults<AspectModel> aspects = await _mediator.Send(query, cancellationToken);

    return View(aspects);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> AspectView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadAspectQuery query = new(parsed ? id : null, idOrSlug);
    AspectModel? aspect = await _mediator.Send(query, cancellationToken);
    if (aspect == null)
    {
      return NotFound();
    }

    return View(aspect);
  }
}
