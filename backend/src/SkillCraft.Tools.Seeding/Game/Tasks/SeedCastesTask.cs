using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedCastesTask : SeedingTask
{
  public override string? Description => "Seeds the castes into the CMS.";

  public LanguageModel Language { get; }

  public SeedCastesTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedCastesTaskHandler : INotificationHandler<SeedCastesTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedCastesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedCastesTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedCastesTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedCastesTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/castes.json", Encoding.UTF8, cancellationToken);
    IEnumerable<CastePayload>? castes = SeedingSerializer.Deserialize<IEnumerable<CastePayload>>(json);
    if (castes != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Caste.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Caste.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      IReadOnlyDictionary<string, Guid> features = await LoadFeaturesAsync(cancellationToken);

      foreach (CastePayload caste in castes)
      {
        string displayText = caste.DisplayName ?? caste.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = caste.UniqueSlug,
          DisplayName = caste.DisplayName,
          Description = caste.Description
        };
        CreateOrReplaceContentCommand command = new(caste.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for caste 'Id={caste.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for caste '{Caste}' (Id={Id}).", language.Locale, displayText, caste.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for caste '{Caste}' (Id={Id}).", language.Locale, displayText, caste.Id);
        }

        payload = new()
        {
          UniqueName = caste.UniqueSlug
        };
        AddFieldValues(payload, fields, caste, features);
        command = new(caste.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for caste 'Id={caste.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for caste '{Caste}' (Id={Id}).", displayText, caste.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for caste '{Caste}' (Id={Id}).", displayText, caste.Id);
        }
      }
    }
  }

  private static void AddFieldValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, CastePayload caste, IReadOnlyDictionary<string, Guid> features)
  {
    if (caste.Skill.HasValue)
    {
      payload.AddFieldValue(fields[Caste.Skill], caste.Skill.Value);
    }
    if (!string.IsNullOrWhiteSpace(caste.WealthRoll))
    {
      payload.AddFieldValue(fields[Caste.WealthRoll], caste.WealthRoll);
    }
    if (!string.IsNullOrWhiteSpace(caste.Features))
    {
      Guid[] contentIds = caste.Features.Split(',')
        .Where(feature => !string.IsNullOrWhiteSpace(feature))
        .Select(feature => features[feature.Trim()])
        .ToArray();
      payload.AddFieldValue(fields[Caste.Features], JsonSerializer.Serialize(contentIds));
    }
  }

  private static async Task<IReadOnlyDictionary<string, Guid>> LoadFeaturesAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/features.json", Encoding.UTF8, cancellationToken);
    IEnumerable<FeaturePayload>? features = SeedingSerializer.Deserialize<IEnumerable<FeaturePayload>>(json);

    Dictionary<string, Guid> results = [];
    if (features != null)
    {
      foreach (FeaturePayload feature in features)
      {
        results[feature.DisplayName ?? feature.UniqueSlug] = feature.Id;
      }
    }
    return results.AsReadOnly();
  }
}
