using FluentValidation;
using FluentValidation.Validators;

namespace SkillCraft.Tools.Core.Validators;

internal class RollValidator<T> : IPropertyValidator<T, string>
{
  public string Name { get; } = "RollValidator";

  public string GetDefaultMessageTemplate(string errorCode)
  {
    return "'{PropertyName}' must be a mathematical expression composed of operators ('+'), positive numbers and rolls. A roll is composed of two positive numbers separated by a 'D' (lowercase or uppercase).";
  }

  public bool IsValid(ValidationContext<T> context, string value)
  {
    string[] parts = value.Split('+');
    return parts.All(part => IsNumber(part) || IsRoll(part));
  }
  private static bool IsNumber(string value) => int.TryParse(value, out int number) && number >= 0;
  private static bool IsRoll(string value)
  {
    string[] parts = value.ToLowerInvariant().Split('d');
    return parts.Length == 2 && parts.All(IsNumber);
  }
}
