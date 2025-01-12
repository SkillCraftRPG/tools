using FluentValidation;
using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Core.Natures.Validators;

internal class CreateOrReplaceNatureValidator : AbstractValidator<CreateOrReplaceNaturePayload>
{
  public CreateOrReplaceNatureValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Attribute.HasValue, () => RuleFor(x => x.Attribute!.Value).IsInEnum());
  }
}
