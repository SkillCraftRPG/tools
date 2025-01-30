using Logitar.Cms.Core.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Educations.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("educations")]
public class EducationController : Controller
{
  private readonly IMediator _mediator;

  public EducationController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> EducationList(CancellationToken cancellationToken)
  {
    SearchEducationsPayload payload = new();
    payload.Sort.Add(new EducationSortOption(EducationSort.DisplayName));
    SearchEducationsQuery query = new(payload);
    SearchResults<EducationModel> educations = await _mediator.Send(query, cancellationToken);

    return View(educations);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> EducationView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadEducationQuery query = new(parsed ? id : null, idOrSlug);
    EducationModel? education = await _mediator.Send(query, cancellationToken);
    if (education == null)
    {
      return NotFound();
    }

    return View(education);
  }
}
