using Logitar.Cms.Core.Search;
using MediatR;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Core.Educations.Queries;

public record SearchEducationsQuery(SearchEducationsPayload Payload) : Activity, IRequest<SearchResults<EducationModel>>;

internal class SearchEducationsQueryHandler : IRequestHandler<SearchEducationsQuery, SearchResults<EducationModel>>
{
  private readonly IEducationQuerier _educationQuerier;

  public SearchEducationsQueryHandler(IEducationQuerier educationQuerier)
  {
    _educationQuerier = educationQuerier;
  }

  public async Task<SearchResults<EducationModel>> Handle(SearchEducationsQuery query, CancellationToken cancellationToken)
  {
    return await _educationQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
