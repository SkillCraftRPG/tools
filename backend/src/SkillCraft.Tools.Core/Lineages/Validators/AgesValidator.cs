using FluentValidation;

namespace SkillCraft.Tools.Core.Lineages.Validators;

public class AgesValidator : AbstractValidator<IAges>
{
  public AgesValidator()
  {
    RuleFor(x => x)
      .Must(x =>
      {
        int[] ages = new int?[] { x.Adolescent, x.Adult, x.Mature, x.Venerable }.Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToArray();
        return ages.Length == 0 || (ages.Length == 4 && ages.SequenceEqual(ages.OrderBy(x => x)));
      })
      .WithErrorCode("AgesValidator")
      .WithMessage("Either no age lower bound must be specified, or all age lower bounds must be specified. When specified age lower bounds, each boundary must be greater than the previous.");
  }
}
