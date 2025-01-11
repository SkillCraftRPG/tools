namespace SkillCraft.Tools.Core.Errors;

public record Error
{
  public string Code { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public List<ErrorData> Data { get; set; } = [];

  public Error()
  {
  }

  public Error(string code, string message, IEnumerable<ErrorData>? data = null)
  {
    Code = code;
    Message = message;

    if (data != null)
    {
      Data.AddRange(data);
    }
  }
}
