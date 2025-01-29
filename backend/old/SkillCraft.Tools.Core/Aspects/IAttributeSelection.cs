namespace SkillCraft.Tools.Core.Aspects;

public interface IAttributeSelection
{
  Ability? Mandatory1 { get; }
  Ability? Mandatory2 { get; }
  Ability? Optional1 { get; }
  Ability? Optional2 { get; }
}
