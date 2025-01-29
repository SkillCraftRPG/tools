using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Queries;

public record SearchLineagesQuery(SearchLineagesPayload Payload) : Activity, IRequest<SearchResults<LineageModel>>;

internal class SearchLineagesQueryHandler : IRequestHandler<SearchLineagesQuery, SearchResults<LineageModel>>
{
  private readonly ILineageQuerier _lineageQuerier;

  public SearchLineagesQueryHandler(ILineageQuerier lineageQuerier)
  {
    _lineageQuerier = lineageQuerier;
  }

  public async Task<SearchResults<LineageModel>> Handle(SearchLineagesQuery query, CancellationToken cancellationToken)
  {
    return await _lineageQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
