using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedNaturesTask : SeedingTask
{
  public override string? Description => "Seeds the natures into the CMS.";

  public LanguageModel Language { get; }

  public SeedNaturesTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedNaturesTaskHandler : INotificationHandler<SeedNaturesTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedNaturesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedNaturesTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedNaturesTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedNaturesTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/natures.json", Encoding.UTF8, cancellationToken);
    IEnumerable<NaturePayload>? natures = SeedingSerializer.Deserialize<IEnumerable<NaturePayload>>(json);
    if (natures != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Nature.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Nature.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      IReadOnlyDictionary<string, Guid> gifts = await LoadGiftsAsync(cancellationToken);

      foreach (NaturePayload nature in natures)
      {
        string displayText = nature.DisplayName ?? nature.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = nature.UniqueSlug,
          DisplayName = nature.DisplayName,
          Description = nature.Description
        };
        CreateOrReplaceContentCommand command = new(nature.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for nature 'Id={nature.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for nature '{Nature}' (Id={Id}).", language.Locale, displayText, nature.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for nature '{Nature}' (Id={Id}).", language.Locale, displayText, nature.Id);
        }

        payload = new()
        {
          UniqueName = nature.UniqueSlug,
          DisplayName = nature.DisplayName
        };
        AddFieldValues(payload, fields, nature, gifts);
        command = new(nature.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for nature 'Id={nature.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for nature '{Nature}' (Id={Id}).", displayText, nature.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for nature '{Nature}' (Id={Id}).", displayText, nature.Id);
        }
      }
    }
  }

  private static void AddFieldValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, NaturePayload nature, IReadOnlyDictionary<string, Guid> gifts)
  {
    if (nature.Attribute.HasValue)
    {
      payload.AddFieldValue(fields[Nature.Attribute], nature.Attribute.Value);
    }
    if (!string.IsNullOrWhiteSpace(nature.Gift))
    {
      payload.AddFieldValue(fields[Nature.Gift], gifts[nature.Gift]);
    }
  }

  private async Task<IReadOnlyDictionary<string, Guid>> LoadGiftsAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/customizations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<CustomizationPayload>? customizations = SeedingSerializer.Deserialize<IEnumerable<CustomizationPayload>>(json);

    Dictionary<string, Guid> results = [];
    if (customizations != null)
    {
      foreach (CustomizationPayload customization in customizations)
      {
        if (customization.Type == CustomizationType.Gift)
        {
          results[customization.DisplayName ?? customization.UniqueSlug] = customization.Id;
        }
      }
    }
    return results.AsReadOnly();
  }
}
