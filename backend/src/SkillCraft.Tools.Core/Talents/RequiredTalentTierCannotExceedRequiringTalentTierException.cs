using Logitar;

namespace SkillCraft.Tools.Core.Talents;

public class RequiredTalentTierCannotExceedRequiringTalentTierException : BadRequestException
{
  private const string ErrorMessage = "The tier of the required talent cannot exceed the tier of the requiring talent.";

  public Guid RequiredTalentId
  {
    get => (Guid)Data[nameof(RequiredTalentId)]!;
    private set => Data[nameof(RequiredTalentId)] = value;
  }
  public int RequiredTalentTier
  {
    get => (int)Data[nameof(RequiredTalentTier)]!;
    private set => Data[nameof(RequiredTalentTier)] = value;
  }
  public Guid RequiringTalentId
  {
    get => (Guid)Data[nameof(RequiringTalentId)]!;
    private set => Data[nameof(RequiringTalentId)] = value;
  }
  public int RequiringTalentTier
  {
    get => (int)Data[nameof(RequiringTalentTier)]!;
    private set => Data[nameof(RequiringTalentTier)] = value;
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
      error.Data[nameof(RequiredTalentId)] = RequiredTalentId;
      error.Data[nameof(RequiredTalentTier)] = RequiredTalentTier;
      error.Data[nameof(RequiringTalentId)] = RequiringTalentId;
      error.Data[nameof(RequiringTalentTier)] = RequiringTalentTier;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public RequiredTalentTierCannotExceedRequiringTalentTierException(Talent requiredTalent, Talent requiringTalent, string propertyName)
    : base(BuildMessage(requiredTalent, requiringTalent, propertyName))
  {
    RequiredTalentId = requiredTalent.Id.ToGuid();
    RequiredTalentTier = requiredTalent.Tier;
    RequiringTalentId = requiringTalent.Id.ToGuid();
    RequiringTalentTier = requiringTalent.Tier;
    PropertyName = propertyName;
  }

  private static string BuildMessage(Talent requiredTalent, Talent requiringTalent, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(RequiredTalentId), requiredTalent.Id.ToGuid())
    .AddData(nameof(RequiredTalentTier), requiredTalent.Tier)
    .AddData(nameof(RequiringTalentId), requiringTalent.Id.ToGuid())
    .AddData(nameof(RequiringTalentTier), requiringTalent.Tier)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
