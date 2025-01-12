using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Talents.Models;

public record TalentSortOption : SortOption
{
  public new TalentSort Field
  {
    get => Enum.Parse<TalentSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public TalentSortOption() : this(TalentSort.DisplayName)
  {
  }

  public TalentSortOption(TalentSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
