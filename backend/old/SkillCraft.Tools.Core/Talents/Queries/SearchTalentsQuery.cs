using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Talents.Queries;

public record SearchTalentsQuery(SearchTalentsPayload Payload) : Activity, IRequest<SearchResults<TalentModel>>;

internal class SearchTalentsQueryHandler : IRequestHandler<SearchTalentsQuery, SearchResults<TalentModel>>
{
  private readonly ITalentQuerier _talentQuerier;

  public SearchTalentsQueryHandler(ITalentQuerier talentQuerier)
  {
    _talentQuerier = talentQuerier;
  }

  public async Task<SearchResults<TalentModel>> Handle(SearchTalentsQuery query, CancellationToken cancellationToken)
  {
    return await _talentQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
