using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Lineages.Models;

public record SearchLineagesPayload : SearchPayload
{
  public Ability? Attribute { get; set; }
  public Guid? LanguageId { get; set; }
  public Guid? ParentId { get; set; }
  public SizeCategory? SizeCategory { get; set; }

  public new List<LineageSortOption> Sort { get; set; } = [];
}
