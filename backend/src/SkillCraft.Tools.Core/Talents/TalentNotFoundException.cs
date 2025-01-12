using Logitar;

namespace SkillCraft.Tools.Core.Talents;

public class TalentNotFoundException : NotFoundException
{
  private const string ErrorMessage = "The specified talent could not be found.";

  public Guid TalentId
  {
    get => (Guid)Data[nameof(TalentId)]!;
    private set => Data[nameof(TalentId)] = value;
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
      error.Data[nameof(TalentId)] = TalentId;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public TalentNotFoundException(TalentId talentId, string propertyName) : base(BuildMessage(talentId, propertyName))
  {
    TalentId = talentId.ToGuid();
    PropertyName = propertyName;
  }

  private static string BuildMessage(TalentId talentId, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(TalentId), talentId.ToGuid())
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
