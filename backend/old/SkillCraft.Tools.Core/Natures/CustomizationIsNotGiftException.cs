using Logitar;
using SkillCraft.Tools.Core.Customizations;

namespace SkillCraft.Tools.Core.Natures;

public class CustomizationIsNotGiftException : BadRequestException
{
  private const string ErrorMessage = "The specified customization is not a gift.";

  public Guid CustomizationId
  {
    get => (Guid)Data[nameof(CustomizationId)]!;
    private set => Data[nameof(CustomizationId)] = value;
  }
  public CustomizationType CustomizationType
  {
    get => (CustomizationType)Data[nameof(CustomizationType)]!;
    private set => Data[nameof(CustomizationType)] = value;
  }
  public string PropertyName
  {
    get => (string?)Data[nameof(PropertyName)]!;
    private set => Data[nameof(PropertyName)] = value;
  }

  public override Error Error
  {
    get
    {
      Error error = new(this.GetErrorCode(), ErrorMessage);
      error.Data[nameof(CustomizationId)] = CustomizationId;
      error.Data[nameof(CustomizationType)] = CustomizationType;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public CustomizationIsNotGiftException(Customization customization, string propertyName)
    : base(BuildMessage(customization, propertyName))
  {
    CustomizationId = customization.Id.ToGuid();
    CustomizationType = customization.Type;
    PropertyName = propertyName;
  }

  private static string BuildMessage(Customization customization, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(CustomizationId), customization.Id.ToGuid())
    .AddData(nameof(CustomizationType), customization.Type)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
