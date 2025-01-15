using Logitar.Portal.Contracts.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Languages.Queries;

namespace SkillCraft.Tools.Controllers;

[Route("langues")]
public class LanguageController : Controller
{
  private readonly IMediator _mediator;

  public LanguageController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet]
  public async Task<ActionResult> LanguageList(CancellationToken cancellationToken)
  {
    SearchLanguagesPayload payload = new();
    payload.Sort.Add(new LanguageSortOption(LanguageSort.DisplayName));
    SearchLanguagesQuery query = new(payload);
    SearchResults<LanguageModel> languages = await _mediator.Send(query, cancellationToken);

    return View(languages);
  }

  [HttpGet("{idOrSlug}")]
  public async Task<ActionResult> LanguageView(string idOrSlug, CancellationToken cancellationToken)
  {
    bool parsed = Guid.TryParse(idOrSlug, out Guid id);
    ReadLanguageQuery query = new(parsed ? id : null, idOrSlug);
    LanguageModel? language = await _mediator.Send(query, cancellationToken);
    if (language == null)
    {
      return NotFound();
    }

    return View(language);
  }
}
