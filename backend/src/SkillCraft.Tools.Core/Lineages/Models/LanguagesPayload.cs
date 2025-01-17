namespace SkillCraft.Tools.Core.Lineages.Models;

public record LanguagesPayload
{
  public List<Guid> Ids { get; set; } = [];
  public int Extra { get; set; }
  public string? Text { get; set; }
}
