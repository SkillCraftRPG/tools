namespace SkillCraft.Tools.Core.Aspects.Models;

public class AspectModel : AggregateModel
{
  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): Attributes
  // TODO(fpion): Skills

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
