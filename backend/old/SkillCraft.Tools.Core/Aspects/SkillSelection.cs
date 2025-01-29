using FluentValidation;
using SkillCraft.Tools.Core.Aspects.Validators;

namespace SkillCraft.Tools.Core.Aspects;

public record SkillSelection : ISkillSelection
{
  public Skill? Discounted1 { get; }
  public Skill? Discounted2 { get; }

  public SkillSelection(ISkillSelection skills) : this(skills.Discounted1, skills.Discounted2)
  {
  }

  [JsonConstructor]
  public SkillSelection(Skill? discounted1 = null, Skill? discounted2 = null)
  {
    Discounted1 = discounted1;
    Discounted2 = discounted2;
    new SkillSelectionValidator().ValidateAndThrow(this);
  }
}
