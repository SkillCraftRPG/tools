using GraphQL.Types;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.GraphQL;

internal class SizeCategoryGraphType : EnumerationGraphType<SizeCategory>
{
  public SizeCategoryGraphType()
  {
    Name = nameof(SizeCategory);
    Description = "Represents the available size categories.";

    AddValue(SizeCategory.Diminutive, string.Empty);
    AddValue(SizeCategory.Tiny, string.Empty);
    AddValue(SizeCategory.Small, string.Empty);
    AddValue(SizeCategory.Medium, string.Empty);
    AddValue(SizeCategory.Large, string.Empty);
    AddValue(SizeCategory.Huge, string.Empty);
    AddValue(SizeCategory.Gargantuan, string.Empty);
    AddValue(SizeCategory.Colossal, string.Empty);
  }
  private void AddValue(SizeCategory value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
