using GraphQL.Types;
using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.GraphQL.Customizations;

internal class CustomizationSortGraphType : EnumerationGraphType<CustomizationSort>
{
  public CustomizationSortGraphType()
  {
    Name = "CustomizationSort";
    Description = "Represents the available customization fields for sorting.";

    AddValue(CustomizationSort.CreatedOn, "The customizations will be sorted by their creation date and time.");
    AddValue(CustomizationSort.DisplayName, "The customizations will be sorted by their display name.");
    AddValue(CustomizationSort.UniqueSlug, "The customizations will be sorted by their unique slug.");
    AddValue(CustomizationSort.UpdatedOn, "The customizations will be sorted by their latest update date and time.");
  }
  private void AddValue(CustomizationSort value, string description)
  {
    Add(value.ToString(), value, description);
  }
}
