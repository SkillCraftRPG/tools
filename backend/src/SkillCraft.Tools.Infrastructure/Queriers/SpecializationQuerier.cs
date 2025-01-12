using SkillCraft.Tools.Core.Specializations;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class SpecializationQuerier : ISpecializationQuerier
{
  public Task<SpecializationModel?> ReadAsync(Guid id, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException(); // ISSUE: https://github.com/SkillCraftRPG/tools/issues/6
  }

  public Task<SpecializationModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException(); // ISSUE: https://github.com/SkillCraftRPG/tools/issues/6
  }
}
