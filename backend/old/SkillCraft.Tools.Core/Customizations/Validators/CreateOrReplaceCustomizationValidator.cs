using FluentValidation;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.Core.Customizations.Validators;

internal class CreateOrReplaceCustomizationValidator : AbstractValidator<CreateOrReplaceCustomizationPayload>
{
  public CreateOrReplaceCustomizationValidator()
  {
    RuleFor(x => x.Type).IsInEnum();

    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());
  }
}
