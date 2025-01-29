using FluentValidation;

namespace SkillCraft.Tools.Core.Educations;

public record WealthMultiplier
{
  public double Value { get; }

  public WealthMultiplier(double value)
  {
    Value = value;
    new Validator().ValidateAndThrow(this);
  }

  private class Validator : AbstractValidator<WealthMultiplier>
  {
    public Validator()
    {
      RuleFor(x => x.Value).WealthMultiplier();
    }
  }
}
