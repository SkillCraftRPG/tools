using Logitar;
using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Customizations;
using SkillCraft.Tools.Core.Educations;
using SkillCraft.Tools.Core.Languages;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Specializations;
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

  public UniqueSlugAlreadyUsedException(Aspect aspect, AspectId conflictId)
    : this(conflictId.StreamId, aspect.Id.StreamId, aspect.UniqueSlug, nameof(aspect.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Caste caste, CasteId conflictId)
    : this(conflictId.StreamId, caste.Id.StreamId, caste.UniqueSlug, nameof(caste.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Customization customization, CustomizationId conflictId)
    : this(conflictId.StreamId, customization.Id.StreamId, customization.UniqueSlug, nameof(customization.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Education education, EducationId conflictId)
    : this(conflictId.StreamId, education.Id.StreamId, education.UniqueSlug, nameof(education.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Language language, LanguageId conflictId)
    : this(conflictId.StreamId, language.Id.StreamId, language.UniqueSlug, nameof(language.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Nature nature, NatureId conflictId)
    : this(conflictId.StreamId, nature.Id.StreamId, nature.UniqueSlug, nameof(nature.UniqueSlug))
  {
  }
  public UniqueSlugAlreadyUsedException(Specialization specialization, SpecializationId conflictId)
    : this(conflictId.StreamId, specialization.Id.StreamId, specialization.UniqueSlug, nameof(specialization.UniqueSlug))
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
