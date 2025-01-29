using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class SizeValidator : AbstractValidator<SizeModel>
{
  public SizeValidator()
  {
    RuleFor(x => x.Category).IsInEnum();
    When(x => !string.IsNullOrWhiteSpace(x.Roll), () => RuleFor(x => x.Roll!).Roll());
  }
}
