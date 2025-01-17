using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Core.Lineages.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("lignees")]
public class LineageController : Controller
{
  private readonly IMediator _mediator;

  public LineageController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> LineageList(CancellationToken cancellationToken)
  {
    SearchLineagesPayload payload = new();
    payload.Sort.Add(new LineageSortOption(LineageSort.DisplayName));
    SearchLineagesQuery query = new(payload);
    SearchResults<LineageModel> lineages = await _mediator.Send(query, cancellationToken);

    return View(lineages);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> LineageView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadLineageQuery query = new(parsed ? id : null, idOrSlug);
    LineageModel? lineage = await _mediator.Send(query, cancellationToken);
    if (lineage == null || lineage.Parent != null)
    {
      return NotFound();
    }

    return View(lineage);
  }
}
