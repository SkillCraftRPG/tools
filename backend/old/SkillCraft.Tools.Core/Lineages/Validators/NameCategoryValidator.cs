using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class NameCategoryValidator : AbstractValidator<NameCategory>
{
  public NameCategoryValidator()
  {
    RuleFor(x => x.Key).NotEmpty();
  }
}
