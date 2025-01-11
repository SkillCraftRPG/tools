using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal class TalentEntity : AggregateEntity
{
  public int TalentId { get; private set; }
  public Guid Id { get; private set; }

  public int Tier { get; private set; }
  public string UniqueSlug { get; private set; } = string.Empty;
  public string UniqueSlugNormalized
  {
    get => Helper.Normalize(UniqueSlug);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public bool AllowMultiplePurchases { get; private set; }
  public TalentEntity? RequiredTalent { get; private set; }
  public int? RequiredTalentId { get; private set; }
  public List<TalentEntity> RequiringTalents { get; private set; } = [];
  public Skill? Skill { get; private set; }

  private TalentEntity()
  {
  }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
