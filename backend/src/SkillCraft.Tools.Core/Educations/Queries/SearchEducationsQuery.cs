using MediatR;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Educations.Queries;

public record SearchEducationsQuery(SearchEducationsPayload Payload) : IRequest<SearchResults<EducationModel>>;

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
