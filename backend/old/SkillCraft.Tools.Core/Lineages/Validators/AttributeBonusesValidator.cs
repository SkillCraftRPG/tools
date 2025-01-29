using FluentValidation;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class AttributeBonusesValidator : AbstractValidator<IAttributeBonuses>
{
  public AttributeBonusesValidator()
  {
    RuleFor(x => x.Agility).InclusiveBetween(0, 2);
    RuleFor(x => x.Coordination).InclusiveBetween(0, 2);
    RuleFor(x => x.Intellect).InclusiveBetween(0, 2);
    RuleFor(x => x.Presence).InclusiveBetween(0, 2);
    RuleFor(x => x.Sensitivity).InclusiveBetween(0, 2);
    RuleFor(x => x.Spirit).InclusiveBetween(0, 2);
    RuleFor(x => x.Vigor).InclusiveBetween(0, 2);

    RuleFor(x => x.Extra).GreaterThanOrEqualTo(0).LessThanOrEqualTo(attributes => GetExtraMaximumValue(attributes));
  }

  private static int GetExtraMaximumValue(IAttributeBonuses attributes)
  {
    int maximum = 7;
    if (attributes.Agility > 0)
    {
      maximum--;
    }
    if (attributes.Coordination > 0)
    {
      maximum--;
    }
    if (attributes.Intellect > 0)
    {
      maximum--;
    }
    if (attributes.Presence > 0)
    {
      maximum--;
    }
    if (attributes.Sensitivity > 0)
    {
      maximum--;
    }
    if (attributes.Spirit > 0)
    {
      maximum--;
    }
    if (attributes.Vigor > 0)
    {
      maximum--;
    }
    return maximum;
  }
}
