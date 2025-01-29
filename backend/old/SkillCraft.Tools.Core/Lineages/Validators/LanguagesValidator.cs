using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class LanguagesValidator : AbstractValidator<LanguagesPayload>
{
  public LanguagesValidator()
  {
    RuleFor(x => x.Extra).InclusiveBetween(0, 3);
    When(x => !string.IsNullOrWhiteSpace(x.Text), () => RuleFor(x => x.Text!).LanguagesText());
  }
}
