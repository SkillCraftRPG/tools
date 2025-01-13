using MediatR;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Castes.Queries;

public record SearchCastesQuery(SearchCastesPayload Payload) : IRequest<SearchResults<CasteModel>>;

internal class SearchCastesQueryHandler : IRequestHandler<SearchCastesQuery, SearchResults<CasteModel>>
{
  private readonly ICasteQuerier _casteQuerier;

  public SearchCastesQueryHandler(ICasteQuerier casteQuerier)
  {
    _casteQuerier = casteQuerier;
  }

  public async Task<SearchResults<CasteModel>> Handle(SearchCastesQuery query, CancellationToken cancellationToken)
  {
    return await _casteQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
