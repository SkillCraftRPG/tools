using GraphQL.Types;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.GraphQL;

internal class SkillGraphType : EnumerationGraphType<Skill>
{
  public SkillGraphType()
  {
    Name = nameof(Skill);
    Description = "Represents the available character skills.";

    AddValue(Skill.Acrobatics, string.Empty);
    AddValue(Skill.Athletics, string.Empty);
    AddValue(Skill.Craft, string.Empty);
    AddValue(Skill.Deception, string.Empty);
    AddValue(Skill.Diplomacy, string.Empty);
    AddValue(Skill.Discipline, string.Empty);
    AddValue(Skill.Insight, string.Empty);
    AddValue(Skill.Investigation, string.Empty);
    AddValue(Skill.Knowledge, string.Empty);
    AddValue(Skill.Linguistics, string.Empty);
    AddValue(Skill.Medicine, string.Empty);
    AddValue(Skill.Melee, string.Empty);
    AddValue(Skill.Occultism, string.Empty);
    AddValue(Skill.Orientation, string.Empty);
    AddValue(Skill.Perception, string.Empty);
    AddValue(Skill.Performance, string.Empty);
    AddValue(Skill.Resistance, string.Empty);
    AddValue(Skill.Stealth, string.Empty);
    AddValue(Skill.Survival, string.Empty);
    AddValue(Skill.Thievery, string.Empty);
  }
  private void AddValue(Skill value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
