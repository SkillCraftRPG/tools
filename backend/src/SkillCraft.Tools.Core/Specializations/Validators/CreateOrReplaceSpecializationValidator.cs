using FluentValidation;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations.Validators;

internal class CreateOrReplaceSpecializationValidator : AbstractValidator<CreateOrReplaceSpecializationPayload>
{
  public CreateOrReplaceSpecializationValidator()
  {
    RuleFor(x => x.Tier).InclusiveBetween(1, 3);

    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.ReservedTalent != null, () => RuleFor(x => x.ReservedTalent!).SetValidator(new ReservedTalentValidator()));
  }
}
