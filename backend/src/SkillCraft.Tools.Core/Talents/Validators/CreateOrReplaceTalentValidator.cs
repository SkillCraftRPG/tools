using FluentValidation;
using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Core.Talents.Validators;

internal class CreateOrReplaceTalentValidator : AbstractValidator<CreateOrReplaceTalentPayload>
{
  public CreateOrReplaceTalentValidator()
  {
    RuleFor(x => x.Tier).InclusiveBetween(0, 3);

    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    When(x => x.Skill.HasValue, () => RuleFor(x => x.Skill!.Value).IsInEnum());
  }
}
