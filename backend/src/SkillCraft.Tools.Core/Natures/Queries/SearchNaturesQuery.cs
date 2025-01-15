using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Core.Natures.Queries;

public record SearchNaturesQuery(SearchNaturesPayload Payload) : IRequest<SearchResults<NatureModel>>;

internal class SearchNaturesQueryHandler : IRequestHandler<SearchNaturesQuery, SearchResults<NatureModel>>
{
  private readonly INatureQuerier _natureQuerier;

  public SearchNaturesQueryHandler(INatureQuerier natureQuerier)
  {
    _natureQuerier = natureQuerier;
  }

  public async Task<SearchResults<NatureModel>> Handle(SearchNaturesQuery query, CancellationToken cancellationToken)
  {
    return await _natureQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
