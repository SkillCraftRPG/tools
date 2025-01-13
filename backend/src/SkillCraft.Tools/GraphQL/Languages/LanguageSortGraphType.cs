using GraphQL.Types;
using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.GraphQL.Languages;

internal class LanguageSortGraphType : EnumerationGraphType<LanguageSort>
{
  public LanguageSortGraphType()
  {
    Name = "LanguageSort";
    Description = "Represents the available language fields for sorting.";

    AddValue(LanguageSort.CreatedOn, "The languages will be sorted by their creation date and time.");
    AddValue(LanguageSort.DisplayName, "The languages will be sorted by their display name.");
    AddValue(LanguageSort.UniqueSlug, "The languages will be sorted by their unique slug.");
    AddValue(LanguageSort.UpdatedOn, "The languages will be sorted by their latest update date and time.");
  }
  private void AddValue(LanguageSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
