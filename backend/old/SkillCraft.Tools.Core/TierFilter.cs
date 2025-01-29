namespace SkillCraft.Tools.Core;

public record TierFilter
{
  public string Operator { get; set; } = string.Empty;
  public List<int> Values { get; set; } = [];

  public TierFilter()
  {
  }

  public TierFilter(string @operator, IEnumerable<int> values)
  {
    Operator = @operator;
    Values.AddRange(values);
  }
}
