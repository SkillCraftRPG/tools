using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Aspects.Models;

public record AspectSortOption : SortOption
{
  public new AspectSort Field
  {
    get => Enum.Parse<AspectSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public AspectSortOption() : this(AspectSort.DisplayName)
  {
  }

  public AspectSortOption(AspectSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
