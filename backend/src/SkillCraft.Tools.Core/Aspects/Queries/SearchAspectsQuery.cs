using MediatR;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Aspects.Queries;

public record SearchAspectsQuery(SearchAspectsPayload Payload) : IRequest<SearchResults<AspectModel>>;

internal class SearchAspectsQueryHandler : IRequestHandler<SearchAspectsQuery, SearchResults<AspectModel>>
{
  private readonly IAspectQuerier _aspectQuerier;

  public SearchAspectsQueryHandler(IAspectQuerier aspectQuerier)
  {
    _aspectQuerier = aspectQuerier;
  }

  public async Task<SearchResults<AspectModel>> Handle(SearchAspectsQuery query, CancellationToken cancellationToken)
  {
    return await _aspectQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
