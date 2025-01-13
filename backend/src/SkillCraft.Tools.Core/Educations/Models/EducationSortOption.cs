using SkillCraft.Tools.Core.Search;

namespace SkillCraft.Tools.Core.Educations.Models;

public record EducationSortOption : SortOption
{
  public new EducationSort Field
  {
    get => Enum.Parse<EducationSort>(base.Field);
    set => base.Field = value.ToString();
  }

  public EducationSortOption() : this(EducationSort.DisplayName)
  {
  }

  public EducationSortOption(EducationSort field, bool isDescending = false) : base(field.ToString(), isDescending)
  {
  }
}
