using FluentValidation;

namespace SkillCraft.Tools.Core.Languages;

public record Script
{
  public const int MaximumLength = byte.MaxValue;

  public string Value { get; }

  public Script(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static Script? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<Script>
  {
    public Validator()
    {
      RuleFor(x => x.Value).Script();
    }
  }
}
