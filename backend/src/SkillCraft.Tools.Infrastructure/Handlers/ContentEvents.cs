using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Events;
using Logitar.Cms.Core.Fields;
using Logitar.Cms.Core.Localization;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Infrastructure.Materialization.Materialize;
using SkillCraft.Tools.Infrastructure.Materialization.Remove;
using Language = Logitar.Cms.Core.Localization.Language;
using LanguageType = SkillCraft.Tools.Core.Contents.Language;

namespace SkillCraft.Tools.Infrastructure.Handlers;

internal class ContentEvents : INotificationHandler<ContentLocalePublished>, INotificationHandler<ContentLocaleUnpublished>
{
  private readonly IContentRepository _contentRepository;
  private readonly IContentTypeRepository _contentTypeRepository;
  private readonly ILanguageRepository _languageRepository;
  private readonly IMediator _mediator;

  public ContentEvents(
    IContentRepository contentRepository,
    IContentTypeRepository contentTypeRepository,
    ILanguageRepository languageRepository,
    IMediator mediator)
  {
    _contentRepository = contentRepository;
    _contentTypeRepository = contentTypeRepository;
    _languageRepository = languageRepository;
    _mediator = mediator;
  }

  public async Task Handle(ContentLocalePublished @event, CancellationToken cancellationToken)
  {
    Language? language = null;
    if (@event.LanguageId.HasValue)
    {
      language = await _languageRepository.LoadAsync(@event.LanguageId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The language 'Id={@event.LanguageId}' was not loaded.");

      if (!language.IsDefault)
      {
        return;
      }
    }

    ContentId contentId = new(@event.StreamId);
    Content content = await _contentRepository.LoadAsync(contentId, cancellationToken)
      ?? throw new InvalidOperationException($"The content 'Id={contentId}' was not loaded.");
    ContentLocale locale = @event.LanguageId.HasValue ? content.FindLocale(@event.LanguageId.Value) : content.Invariant;

    ContentType contentType = await _contentTypeRepository.LoadAsync(content, cancellationToken);
    IReadOnlyDictionary<string, string> fieldValues = BuildFieldValues(locale, language, contentType);

    IRequest? command = null;
    switch (contentType.UniqueName.Value)
    {
      case Aspect.UniqueName:
        command = new MaterializeAspectCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case CasteFeature.UniqueName:
        command = new MaterializeFeatureCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Caste.UniqueName:
        command = new MaterializeCasteCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Customization.UniqueName:
        command = new MaterializeCustomizationCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Education.UniqueName:
        command = new MaterializeEducationCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case LanguageType.UniqueName:
        command = new MaterializeLanguageCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Lineage.UniqueName:
        command = new MaterializeLineageCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case LineageTrait.UniqueName:
        command = new MaterializeTraitCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Nature.UniqueName:
        command = new MaterializeNatureCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Script.UniqueName:
        command = new MaterializeScriptCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Specialization.UniqueName:
        command = new MaterializeSpecializationCommand(@event, fieldValues, language == null ? null : locale);
        break;
      case Talent.UniqueName:
        command = new MaterializeTalentCommand(@event, fieldValues, language == null ? null : locale);
        break;
    }
    if (command != null)
    {
      await _mediator.Send(command, cancellationToken);
    }
  }
  private static IReadOnlyDictionary<string, string> BuildFieldValues(ContentLocale locale, Language? language, ContentType contentType)
  {
    bool isInvariant = language == null;
    Dictionary<Guid, FieldDefinition> fieldDefinitions = contentType.FieldDefinitions
      .Where(x => x.IsInvariant == isInvariant)
      .ToDictionary(x => x.Id, x => x);

    Dictionary<string, string> fieldValues = new(capacity: locale.FieldValues.Count);
    foreach (KeyValuePair<Guid, string> fieldValue in locale.FieldValues)
    {
      if (fieldDefinitions.TryGetValue(fieldValue.Key, out FieldDefinition? fieldDefinition))
      {
        fieldValues[fieldDefinition.UniqueName.Value] = fieldValue.Value;
      }
    }
    return fieldValues.AsReadOnly();
  }

  public async Task Handle(ContentLocaleUnpublished @event, CancellationToken cancellationToken)
  {
    Language? language = null;
    if (@event.LanguageId.HasValue)
    {
      language = await _languageRepository.LoadAsync(@event.LanguageId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The language 'Id={@event.LanguageId}' was not loaded.");

      if (!language.IsDefault)
      {
        return;
      }
    }

    ContentId contentId = new(@event.StreamId);
    Content content = await _contentRepository.LoadAsync(contentId, cancellationToken)
      ?? throw new InvalidOperationException($"The content 'Id={contentId}' was not loaded.");

    ContentType contentType = await _contentTypeRepository.LoadAsync(content, cancellationToken); ;

    IRequest? command = null;
    switch (contentType.UniqueName.Value)
    {
      case Aspect.UniqueName:
        command = new RemoveAspectCommand(@event);
        break;
      case CasteFeature.UniqueName:
        command = new RemoveFeatureCommand(@event);
        break;
      case Caste.UniqueName:
        command = new RemoveCasteCommand(@event);
        break;
      case Customization.UniqueName:
        command = new RemoveCustomizationCommand(@event);
        break;
      case Education.UniqueName:
        command = new RemoveEducationCommand(@event);
        break;
      case LanguageType.UniqueName:
        command = new RemoveLanguageCommand(@event);
        break;
      case Lineage.UniqueName:
        command = new RemoveLineageCommand(@event);
        break;
      case LineageTrait.UniqueName:
        command = new RemoveTraitCommand(@event);
        break;
      case Nature.UniqueName:
        command = new RemoveNatureCommand(@event);
        break;
      case Script.UniqueName:
        command = new RemoveScriptCommand(@event);
        break;
      case Specialization.UniqueName:
        command = new RemoveSpecializationCommand(@event);
        break;
      case Talent.UniqueName:
        command = new RemoveTalentCommand(@event);
        break;
    }
    if (command != null)
    {
      await _mediator.Send(command, cancellationToken);
    }
  }
}
