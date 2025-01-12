using Logitar;

namespace SkillCraft.Tools.Core.Customizations;

public class CustomizationNotFoundException : NotFoundException
{
  private const string ErrorMessage = "The specified customization could not be found.";

  public Guid CustomizationId
  {
    get => (Guid)Data[nameof(CustomizationId)]!;
    private set => Data[nameof(CustomizationId)] = value;
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
      error.Data[nameof(CustomizationId)] = CustomizationId;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public CustomizationNotFoundException(CustomizationId customizationId, string propertyName) : base(BuildMessage(customizationId, propertyName))
  {
    CustomizationId = customizationId.ToGuid();
    PropertyName = propertyName;
  }

  private static string BuildMessage(CustomizationId customizationId, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(CustomizationId), customizationId.ToGuid())
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
