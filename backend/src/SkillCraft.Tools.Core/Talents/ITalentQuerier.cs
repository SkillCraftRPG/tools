using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Talents;

public interface ITalentQuerier
{
  Task<TalentModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default);
  Task<TalentModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default);
}
