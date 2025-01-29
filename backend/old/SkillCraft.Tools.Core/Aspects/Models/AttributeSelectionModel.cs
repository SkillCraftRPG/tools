namespace SkillCraft.Tools.Core.Aspects.Models;

public record AttributeSelectionModel : IAttributeSelection
{
  public Ability? Mandatory1 { get; set; }
  public Ability? Mandatory2 { get; set; }
  public Ability? Optional1 { get; set; }
  public Ability? Optional2 { get; set; }
}
