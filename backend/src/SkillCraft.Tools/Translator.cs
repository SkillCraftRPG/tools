using SkillCraft.Tools.Core;

namespace SkillCraft.Tools;

public static class Translator
{
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

  public static string Translate(Skill skill) => _skills.TryGetValue(skill, out string? translation) ? translation : skill.ToString();
}
