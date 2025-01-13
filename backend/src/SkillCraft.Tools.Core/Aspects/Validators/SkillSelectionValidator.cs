using FluentValidation;
using FluentValidation.Results;

namespace SkillCraft.Tools.Core.Aspects.Validators;

internal class SkillSelectionValidator : AbstractValidator<ISkillSelection>
{
  public SkillSelectionValidator()
  {
    When(x => x.Discounted1.HasValue, () => RuleFor(x => x.Discounted1!.Value).IsInEnum());
    When(x => x.Discounted2.HasValue, () => RuleFor(x => x.Discounted2!.Value).IsInEnum());
  }

  public override ValidationResult Validate(ValidationContext<ISkillSelection> context)
  {
    const string errorMessage = "Each property must specify a different skill. A skill can only be specified by one property.";

    ValidationResult result = base.Validate(context);

    ISkillSelection skills = context.InstanceToValidate;
    if (skills.Discounted1.HasValue && skills.Discounted1.Value == skills.Discounted2)
    {
      result.Errors.Add(new ValidationFailure(nameof(skills.Discounted1), errorMessage, skills.Discounted1.Value)
      {
        ErrorCode = "SkillSelectionValidator"
      });
      result.Errors.Add(new ValidationFailure(nameof(skills.Discounted2), errorMessage, skills.Discounted2.Value)
      {
        ErrorCode = "SkillSelectionValidator"
      });
    }

    return result;
  }
}
