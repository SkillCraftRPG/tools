using Logitar;

namespace SkillCraft.Tools.Core.Lineages;

public class InvalidParentLineageException : BadRequestException
{
  private const string ErrorMessage = "The specified parent lineage has a parent lineage.";

  public Guid LineageId
  {
    get => (Guid)Data[nameof(LineageId)]!;
    private set => Data[nameof(LineageId)] = value;
  }
  public string PropertyName
  {
    get => (string)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public override Error Error
  {
    get
    {
      Error error = new(this.GetErrorCode(), ErrorMessage);
      error.Data[nameof(LineageId)] = LineageId;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public InvalidParentLineageException(Lineage lineage, string propertyName) : base(BuildMessage(lineage, propertyName))
  {
    LineageId = lineage.Id.ToGuid();
    PropertyName = propertyName;
  }

  private static string BuildMessage(Lineage parent, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(LineageId), parent.Id.ToGuid())
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
