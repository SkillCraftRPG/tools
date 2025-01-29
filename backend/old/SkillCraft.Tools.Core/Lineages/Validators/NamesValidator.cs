using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class NamesValidator : AbstractValidator<NamesModel>
{
  public NamesValidator()
  {
    When(x => !string.IsNullOrWhiteSpace(x.Text), () => RuleFor(x => x.Text!).NamesText());
    RuleForEach(x => x.Custom).SetValidator(new NameCategoryValidator());
  }
}
