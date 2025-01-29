using FluentValidation;

namespace SkillCraft.Tools.Core.Specializations;

public record OtherOption
{
  public string Value { get; }

  public OtherOption(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static OtherOption? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<OtherOption>
  {
    public Validator()
    {
      RuleFor(x => x.Value).OtherOption();
    }
  }
}
