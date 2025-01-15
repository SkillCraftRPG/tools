using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools;

public static class Translator
{
  private static readonly Dictionary<Ability, string> _attributes = new()
  {
    [Ability.Agility] = "Agilité",
    [Ability.Coordination] = "Coordination",
    [Ability.Intellect] = "Intellect",
    [Ability.Presence] = "Présence",
    [Ability.Sensitivity] = "Sensibilité",
    [Ability.Spirit] = "Esprit",
    [Ability.Vigor] = "Vigueur"
  };
  private static readonly Dictionary<CustomizationType, string> _customizationTypes = new()
  {
    [CustomizationType.Disability] = "Handicap",
    [CustomizationType.Gift] = "Don"
  };
  private static readonly Dictionary<Skill, string> _skills = new()
  {
    [Skill.Acrobatics] = "Acrobaties",
    [Skill.Athletics] = "Athlétisme",
    [Skill.Craft] = "Artisanat",
    [Skill.Deception] = "Tromperie",
    [Skill.Diplomacy] = "Diplomatie",
    [Skill.Discipline] = "Discipline",
    [Skill.Insight] = "Intuition",
    [Skill.Investigation] = "Investigation",
    [Skill.Knowledge] = "Connaissance",
    [Skill.Linguistics] = "Linguistique",
    [Skill.Medicine] = "Médecine",
    [Skill.Melee] = "Mêlée",
    [Skill.Occultism] = "Occultisme",
    [Skill.Orientation] = "Orientation",
    [Skill.Perception] = "Perception",
    [Skill.Performance] = "Performance",
    [Skill.Resistance] = "Résistance",
    [Skill.Stealth] = "Furtivité",
    [Skill.Survival] = "Survie",
    [Skill.Thievery] = "Roublardise"
  };

  public static string Translate(Ability attribute) => _attributes.TryGetValue(attribute, out string? translation) ? translation : attribute.ToString();
  public static string Translate(CustomizationType type) => _customizationTypes.TryGetValue(type, out string? translation) ? translation : type.ToString();
  public static string Translate(Skill skill) => _skills.TryGetValue(skill, out string? translation) ? translation : skill.ToString();
}
