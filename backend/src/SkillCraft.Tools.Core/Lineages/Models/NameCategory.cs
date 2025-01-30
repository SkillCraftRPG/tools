namespace SkillCraft.Tools.Core.Lineages.Models;

public record NameCategory
{
  public string Key { get; set; } = string.Empty;
  public List<string> Values { get; set; } = [];

  public NameCategory()
  {
  }

  public NameCategory(KeyValuePair<string, IEnumerable<string>> category) : this(category.Key, category.Value)
  {
  }

  public NameCategory(string key, IEnumerable<string>? values = null)
  {
    Key = key;

    if (values != null)
    {
      Values.AddRange(values);
    }
  }
}
