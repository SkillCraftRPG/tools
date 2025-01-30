using Logitar.Cms.Core;
using MediatR;
using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Core.Languages.Queries;

public record ReadLanguageQuery(Guid? Id, string? Slug) : Activity, IRequest<LanguageModel?>;

internal class ReadLanguageQueryHandler : IRequestHandler<ReadLanguageQuery, LanguageModel?>
{
  private readonly ILanguageQuerier _languageQuerier;

  public ReadLanguageQueryHandler(ILanguageQuerier languageQuerier)
  {
    _languageQuerier = languageQuerier;
  }

  public async Task<LanguageModel?> Handle(ReadLanguageQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, LanguageModel> languages = new(capacity: 2);

    if (query.Id.HasValue)
    {
      var language = await _languageQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (language != null)
      {
        languages[language.Id] = language;
      }
    }
    if (!string.IsNullOrWhiteSpace(query.Slug))
    {
      var language = await _languageQuerier.ReadAsync(query.Slug, cancellationToken);
      if (language != null)
      {
        languages[language.Id] = language;
      }
    }

    if (languages.Count > 1)
    {
      throw TooManyResultsException<LanguageModel>.ExpectedSingle(languages.Count);
    }

    return languages.Values.SingleOrDefault();
  }
}
