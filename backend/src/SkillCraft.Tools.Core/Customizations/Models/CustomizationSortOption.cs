using Logitar.Portal.Contracts.Search;

namespace SkillCraft.Tools.Core.Customizations.Models;

public record CustomizationSortOption : SortOption
{
  public new CustomizationSort Field
  {
    get => Enum.Parse<CustomizationSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public CustomizationSortOption() : this(CustomizationSort.DisplayName)
  {
  }

  public CustomizationSortOption(CustomizationSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
