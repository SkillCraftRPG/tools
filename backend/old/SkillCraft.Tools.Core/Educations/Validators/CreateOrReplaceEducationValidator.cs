using FluentValidation;
using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Core.Educations.Validators;

internal class CreateOrReplaceEducationValidator : AbstractValidator<CreateOrReplaceEducationPayload>
{
  public CreateOrReplaceEducationValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Skill.HasValue, () => RuleFor(x => x.Skill!.Value).IsInEnum());
    When(x => x.WealthMultiplier.HasValue, () => RuleFor(x => x.WealthMultiplier!.Value).WealthMultiplier());
  }
}
