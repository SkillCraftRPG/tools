using Logitar.Portal.Contracts.Search;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Core.Educations;

public interface IEducationQuerier
{
  Task<EducationId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken = default);

  Task<EducationModel> ReadAsync(Education education, CancellationToken cancellationToken = default);
  Task<EducationModel?> ReadAsync(EducationId id, CancellationToken cancellationToken = default);
  Task<EducationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<EducationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<EducationModel>> SearchAsync(SearchEducationsPayload payload, CancellationToken cancellationToken = default);
}
