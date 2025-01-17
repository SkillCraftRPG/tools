using FluentValidation;
using Logitar;

namespace SkillCraft.Tools.Core.Lineages;

public record Names
{
  public const int MaximumLength = ushort.MaxValue;

  public string? Text { get; }
  public IReadOnlyCollection<string> Family { get; }
  public IReadOnlyCollection<string> Female { get; }
  public IReadOnlyCollection<string> Male { get; }
  public IReadOnlyCollection<string> Unisex { get; }
  public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Custom { get; }

  public Names()
  {
    Family = [];
    Female = [];
    Male = [];
    Unisex = [];
    Custom = new Dictionary<string, IReadOnlyCollection<string>>().AsReadOnly();
  }

  [JsonConstructor]
  public Names(
    string? text,
    IReadOnlyCollection<string> family,
    IReadOnlyCollection<string> female,
    IReadOnlyCollection<string> male,
    IReadOnlyCollection<string> unisex,
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> custom)
  {
    Text = text?.CleanTrim();
    Family = Clean(family);
    Female = Clean(female);
    Male = Clean(male);
    Unisex = Clean(unisex);

    Dictionary<string, List<string>> customNames = new(capacity: custom.Count);
    foreach (KeyValuePair<string, IReadOnlyCollection<string>> category in custom)
    {
      IReadOnlyCollection<string> values = Clean(category.Value);
      if (values.Count > 0)
      {
        customNames[category.Key.Trim()] = [.. values];
      }
    }
    Custom = customNames.ToDictionary(x => x.Key, x => (IReadOnlyCollection<string>)x.Value.AsReadOnly()).AsReadOnly();

    new Validator().ValidateAndThrow(this);
  }

  private static IReadOnlyCollection<string> Clean(IEnumerable<string> names) => names
    .Where(name => !string.IsNullOrWhiteSpace(name))
    .Select(name => name.Trim())
    .Distinct()
    .OrderBy(name => name)
    .ToList()
    .AsReadOnly();

  private class Validator : AbstractValidator<Names>
  {
    public Validator()
    {
      When(x => x.Text != null, () => RuleFor(x => x.Text!).NamesText());
      RuleForEach(x => x.Family).NotEmpty();
      RuleForEach(x => x.Female).NotEmpty();
      RuleForEach(x => x.Male).NotEmpty();
      RuleForEach(x => x.Unisex).NotEmpty();
      RuleForEach(x => x.Custom.Keys).NotEmpty();
      RuleForEach(x => x.Custom.Values).NotEmpty();
    }
  }
}
