using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Natures.Models;

public record NatureSortOption : SortOption
{
  public new NatureSort Field
  {
    get => Enum.Parse<NatureSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public NatureSortOption() : this(NatureSort.DisplayName)
  {
  }

  public NatureSortOption(NatureSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
