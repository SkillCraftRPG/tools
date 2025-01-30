using Logitar.Cms.Core.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Castes.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("castes")]
public class CasteController : Controller
{
  private readonly IMediator _mediator;

  public CasteController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> CasteList(CancellationToken cancellationToken)
  {
    SearchCastesPayload payload = new();
    payload.Sort.Add(new CasteSortOption(CasteSort.DisplayName));
    SearchCastesQuery query = new(payload);
    SearchResults<CasteModel> castes = await _mediator.Send(query, cancellationToken);

    return View(castes);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> CasteView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadCasteQuery query = new(parsed ? id : null, idOrSlug);
    CasteModel? caste = await _mediator.Send(query, cancellationToken);
    if (caste == null)
    {
      return NotFound();
    }

    return View(caste);
  }
}
