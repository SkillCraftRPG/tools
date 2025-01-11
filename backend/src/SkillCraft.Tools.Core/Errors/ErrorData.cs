namespace SkillCraft.Tools.Core.Errors;

public record ErrorData
{
  public string Key { get; set; } = string.Empty;
  public object? Value { get; set; }

  public ErrorData()
  {
  }

  public ErrorData(KeyValuePair<string, object?> data) : this(data.Key, data.Value)
  {
  }

  public ErrorData(string key, object? value)
  {
    Key = key;
    Value = value;
  }
}
