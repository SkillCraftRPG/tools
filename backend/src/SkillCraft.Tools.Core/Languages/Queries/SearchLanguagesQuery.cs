using MediatR;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Languages.Queries;

public record SearchLanguagesQuery(SearchLanguagesPayload Payload) : IRequest<SearchResults<LanguageModel>>;

internal class SearchLanguagesQueryHandler : IRequestHandler<SearchLanguagesQuery, SearchResults<LanguageModel>>
{
  private readonly ILanguageQuerier _casteQuerier;

  public SearchLanguagesQueryHandler(ILanguageQuerier casteQuerier)
  {
    _casteQuerier = casteQuerier;
  }

  public async Task<SearchResults<LanguageModel>> Handle(SearchLanguagesQuery query, CancellationToken cancellationToken)
  {
    return await _casteQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
