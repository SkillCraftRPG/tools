using Logitar.Cms.Core.Search;
using MediatR;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations.Queries;

public record SearchSpecializationsQuery(SearchSpecializationsPayload Payload) : Activity, IRequest<SearchResults<SpecializationModel>>;

internal class SearchSpecializationsQueryHandler : IRequestHandler<SearchSpecializationsQuery, SearchResults<SpecializationModel>>
{
  private readonly ISpecializationQuerier _specializationQuerier;

  public SearchSpecializationsQueryHandler(ISpecializationQuerier specializationQuerier)
  {
    _specializationQuerier = specializationQuerier;
  }

  public async Task<SearchResults<SpecializationModel>> Handle(SearchSpecializationsQuery query, CancellationToken cancellationToken)
  {
    return await _specializationQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
