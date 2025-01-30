using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Customizations;
using Attribute = SkillCraft.Tools.Core.Attribute;

namespace SkillCraft.Tools.Models;

public static class Translator
{
  private static readonly Dictionary<Attribute, string> _attributes = new()
  {
    [Attribute.Agility] = "Agilité",
    [Attribute.Coordination] = "Coordination",
    [Attribute.Intellect] = "Intellect",
    [Attribute.Presence] = "Présence",
    [Attribute.Sensitivity] = "Sensibilité",
    [Attribute.Spirit] = "Esprit",
    [Attribute.Vigor] = "Vigueur"
  };
  private static readonly Dictionary<CustomizationType, string> _customizationTypes = new()
  {
    [CustomizationType.Disability] = "Handicap",
    [CustomizationType.Gift] = "Don"
  };
  private static readonly Dictionary<SizeCategory, string> _sizeCategories = new()
  {
    [SizeCategory.Diminutive] = "Infime",
    [SizeCategory.Tiny] = "Minuscule",
    [SizeCategory.Small] = "Petite",
    [SizeCategory.Medium] = "Moyenne",
    [SizeCategory.Large] = "Grande",
    [SizeCategory.Huge] = "Énorme",
    [SizeCategory.Gargantuan] = "Gigantesque",
    [SizeCategory.Colossal] = "Colossale"
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

  public static string Translate(Attribute attribute) => _attributes.TryGetValue(attribute, out string? translation) ? translation : attribute.ToString();
  public static string Translate(CustomizationType type) => _customizationTypes.TryGetValue(type, out string? translation) ? translation : type.ToString();
  public static string Translate(SizeCategory sizeCategory) => _sizeCategories.TryGetValue(sizeCategory, out string? translation) ? translation : sizeCategory.ToString();
  public static string Translate(Skill skill) => _skills.TryGetValue(skill, out string? translation) ? translation : skill.ToString();
}
