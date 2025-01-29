using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("talents")]
public class TalentController : Controller
{
  private readonly IMediator _mediator;

  public TalentController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> TalentList(CancellationToken cancellationToken)
  {
    SearchTalentsPayload payload = new();
    SearchTalentsQuery query = new(payload);
    SearchResults<TalentModel> talents = await _mediator.Send(query, cancellationToken);

    return View(talents);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> TalentView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadTalentQuery query = new(parsed ? id : null, idOrSlug);
    TalentModel? talent = await _mediator.Send(query, cancellationToken);
    if (talent == null)
    {
      return NotFound();
    }

    return View(talent);
  }
}
