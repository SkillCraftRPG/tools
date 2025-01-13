using FluentValidation;
using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.Core.Aspects.Validators;

internal class CreateOrReplaceAspectValidator : AbstractValidator<CreateOrReplaceAspectPayload>
{
  public CreateOrReplaceAspectValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    RuleFor(x => x.Attributes).SetValidator(new AttributeSelectionValidator());
    RuleFor(x => x.Skills).SetValidator(new SkillSelectionValidator());
  }
}
