using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedFeaturesTask : SeedingTask
{
  public override string? Description => "Seeds the caste features into the CMS.";

  public LanguageModel Language { get; }

  public SeedFeaturesTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedFeaturesTaskHandler : INotificationHandler<SeedFeaturesTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedFeaturesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedFeaturesTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedFeaturesTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedFeaturesTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/features.json", Encoding.UTF8, cancellationToken);
    IEnumerable<FeaturePayload>? features = SeedingSerializer.Deserialize<IEnumerable<FeaturePayload>>(json);
    if (features != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(CasteFeature.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{CasteFeature.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      foreach (FeaturePayload feature in features)
      {
        string displayText = feature.DisplayName ?? feature.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = feature.UniqueSlug,
          DisplayName = feature.DisplayName,
          Description = feature.Description
        };
        CreateOrReplaceContentCommand command = new(feature.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for caste feature 'Id={feature.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for caste feature '{Feature}' (Id={Id}).", language.Locale, displayText, feature.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for caste feature '{Feature}' (Id={Id}).", language.Locale, displayText, feature.Id);
        }

        payload = new()
        {
          UniqueName = feature.UniqueSlug
        };
        command = new(feature.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for caste feature 'Id={feature.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for caste feature '{Feature}' (Id={Id}).", displayText, feature.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for caste feature '{Feature}' (Id={Id}).", displayText, feature.Id);
        }
      }
    }
  }
}
