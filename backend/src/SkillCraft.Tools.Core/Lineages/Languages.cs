using FluentValidation;
using Logitar;
using SkillCraft.Tools.Core.Languages;

namespace SkillCraft.Tools.Core.Lineages;

public record Languages
{
  public const int MaximumLength = ushort.MaxValue;

  public IReadOnlyCollection<LanguageId> Ids { get; }
  public int Extra { get; }
  public string? Text { get; }

  public Languages()
  {
    Ids = [];
  }

  public Languages(IEnumerable<Language> languages, int extra, string? text)
    : this(languages.Select(language => language.Id), extra, text)
  {
  }

  public Languages(IEnumerable<LanguageId> ids, int extra, string? text)
    : this(ids.ToArray(), extra, text)
  {
  }

  [JsonConstructor]
  public Languages(IReadOnlyCollection<LanguageId> ids, int extra, string? text)
  {
    Ids = ids.Distinct().ToList().AsReadOnly();
    Extra = extra;
    Text = text?.CleanTrim();
    new Validator().ValidateAndThrow(this);
  }

  private class Validator : AbstractValidator<Languages>
  {
    public Validator()
    {
      RuleFor(x => x.Extra).InclusiveBetween(0, 3);
      When(x => x.Text != null, () => RuleFor(x => x.Text!).LanguagesText());
    }
  }
}
