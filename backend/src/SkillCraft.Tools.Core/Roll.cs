using FluentValidation;

namespace SkillCraft.Tools.Core;

public record Roll
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public Roll(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static Roll? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Roll>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Roll();
    }
  }
}
