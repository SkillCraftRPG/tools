using FluentValidation;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Core.Lineages.Validators;

internal class CreateOrReplaceLineageValidator : AbstractValidator<CreateOrReplaceLineagePayload>
{
  public CreateOrReplaceLineageValidator()
  {
    RuleFor(x => x.UniqueSlug).Slug();
    When(x => !string.IsNullOrWhiteSpace(x.DisplayName), () => RuleFor(x => x.DisplayName!).DisplayName());
    When(x => !string.IsNullOrWhiteSpace(x.Description), () => RuleFor(x => x.Description!).Description());

    RuleFor(x => x.Attributes).SetValidator(new AttributeBonusesValidator());
    RuleForEach(x => x.Traits).SetValidator(new TraitValidator());

    RuleFor(x => x.Languages).SetValidator(new LanguagesValidator());
    RuleFor(x => x.Names).SetValidator(new NamesValidator());

    RuleFor(x => x.Speeds).SetValidator(new SpeedsValidator());
    RuleFor(x => x.Size).SetValidator(new SizeValidator());
    RuleFor(x => x.Weight).SetValidator(new WeightValidator());
    RuleFor(x => x.Ages).SetValidator(new AgesValidator());
  }
}
