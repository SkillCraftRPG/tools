using FluentValidation;
using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Core.Specializations.Validators;

internal class ReservedTalentValidator : AbstractValidator<ReservedTalentModel>
{
  public ReservedTalentValidator()
  {
    RuleFor(x => x.Name).DisplayName();
  }
}
