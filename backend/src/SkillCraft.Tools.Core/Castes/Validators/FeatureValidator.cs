using FluentValidation;
using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Core.Castes.Validators;

internal class FeatureValidator : AbstractValidator<FeaturePayload>
{
  public FeatureValidator()
  {
    RuleFor(x => x.Name).DisplayName();
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());
  }
}
