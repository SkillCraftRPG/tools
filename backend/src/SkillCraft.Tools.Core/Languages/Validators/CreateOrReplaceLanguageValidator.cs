using FluentValidation;
using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Core.Languages.Validators;

internal class CreateOrReplaceLanguageValidator : AbstractValidator<CreateOrReplaceLanguagePayload>
{
  public CreateOrReplaceLanguageValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => !string.IsNullOrWhiteSpace(x.Script), () => RuleFor(x => x.Script!).Script());
    When(x => !string.IsNullOrWhiteSpace(x.TypicalSpeakers), () => RuleFor(x => x.TypicalSpeakers!).TypicalSpeakers());
  }
}
