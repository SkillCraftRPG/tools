using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations;

public interface ISpecializationQuerier
{
  Task<SpecializationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<SpecializationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);
}
