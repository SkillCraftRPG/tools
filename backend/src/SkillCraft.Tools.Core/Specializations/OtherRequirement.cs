using FluentValidation;

namespace SkillCraft.Tools.Core.Specializations;

public record OtherRequirement
{
  public string Value { get; }

  public OtherRequirement(string value)
  {
    Value = value.Trim();
    new Validator().ValidateAndThrow(this);
  }

  public static OtherRequirement? TryCreate(string? value) => string.IsNullOrWhiteSpace(value) ? null : new(value);

  public override string ToString() => Value;

  private class Validator : AbstractValidator<OtherRequirement>
  {
    public Validator()
    {
      RuleFor(x => x.Value).OtherRequirement();
    }
  }
}
