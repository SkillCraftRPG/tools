using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Lineages.Models;

public record LineageSortOption : SortOption
{
  public new LineageSort Field
  {
    get => Enum.Parse<LineageSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public LineageSortOption() : this(LineageSort.DisplayName)
  {
  }

  public LineageSortOption(LineageSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
