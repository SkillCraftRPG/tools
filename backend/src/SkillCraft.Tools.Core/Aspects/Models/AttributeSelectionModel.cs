namespace SkillCraft.Tools.Core.Aspects.Models;

public record AttributeSelectionModel
{
  public Attribute? Mandatory1 { get; set; }
  public Attribute? Mandatory2 { get; set; }
  public Attribute? Optional1 { get; set; }
  public Attribute? Optional2 { get; set; }
}
