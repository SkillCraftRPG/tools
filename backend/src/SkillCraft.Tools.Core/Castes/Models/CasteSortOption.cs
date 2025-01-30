using Logitar.Cms.Core.Search;

namespace SkillCraft.Tools.Core.Castes.Models;

public record CasteSortOption : SortOption
{
  public new CasteSort Field
  {
    get => Enum.Parse<CasteSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public CasteSortOption() : this(CasteSort.DisplayName)
  {
  }

  public CasteSortOption(CasteSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
