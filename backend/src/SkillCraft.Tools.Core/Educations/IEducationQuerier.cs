using Logitar.Cms.Core.Search;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Core.Educations;

public interface IEducationQuerier
{
  Task<EducationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<EducationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);

  Task<SearchResults<EducationModel>> SearchAsync(SearchEducationsPayload payload, CancellationToken cancellationToken = default);
}
