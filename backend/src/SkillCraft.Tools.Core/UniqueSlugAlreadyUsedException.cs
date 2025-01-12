using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Core;

public class UniqueSlugAlreadyUsedException : ConflictException
{
  private const string ErrorMessage = "The specified unique slug is already used.";

  public Guid ConflictId
  {
    get => (Guid)Data[nameof(ConflictId)]!;
    private set => Data[nameof(ConflictId)] = value;
  }
  public Guid EntityId
  {
    get => (Guid)Data[nameof(EntityId)]!;
    private set => Data[nameof(EntityId)] = value;
  }
  public string UniqueSlug
  {
    get => (string)Data[nameof(UniqueSlug)]!;
    private set => Data[nameof(UniqueSlug)] = value;
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
      error.Data[nameof(ConflictId)] = ConflictId;
      error.Data[nameof(EntityId)] = EntityId;
      error.Data[nameof(UniqueSlug)] = UniqueSlug;
      error.Data[nameof(PropertyName)] = PropertyName;
      return error;
    }
  }

  public UniqueSlugAlreadyUsedException(Customization customization, CustomizationId conflictId)
    : this(conflictId.StreamId, customization.Id.StreamId, customization.UniqueSlug, nameof(customization.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Talent talent, TalentId conflictId)
    : this(conflictId.StreamId, talent.Id.StreamId, talent.UniqueSlug, nameof(talent.UniqueSlug))
  {
  }

  private UniqueSlugAlreadyUsedException(StreamId conflictId, StreamId entityId, Slug uniqueSlug, string propertyName)
    : base(BuildMessage(conflictId, entityId, uniqueSlug, propertyName))
  {
    ConflictId = conflictId.ToGuid();
    EntityId = entityId.ToGuid();
    UniqueSlug = uniqueSlug.Value;
    PropertyName = propertyName;
  }

  private static string BuildMessage(StreamId conflictId, StreamId entityId, Slug uniqueSlug, string propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ConflictId), conflictId.ToGuid())
    .AddData(nameof(EntityId), entityId.ToGuid())
    .AddData(nameof(UniqueSlug), uniqueSlug)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
