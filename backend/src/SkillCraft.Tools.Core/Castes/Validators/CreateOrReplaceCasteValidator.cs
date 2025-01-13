using FluentValidation;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Core.Castes.Validators;

internal class CreateOrReplaceCasteValidator : AbstractValidator<CreateOrReplaceCastePayload>
{
  public CreateOrReplaceCasteValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Skill.HasValue, () => RuleFor(x => x.Skill!.Value).IsInEnum());
    When(x => !string.IsNullOrWhiteSpace(x.WealthRoll), () => RuleFor(x => x.WealthRoll!).Roll());

    // TODO(fpion): Features
  }
}
