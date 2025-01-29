using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedCustomizationsTask : SeedingTask
{
  public override string? Description => "Seeds the customizations into the CMS.";

  public LanguageModel Language { get; }

  public SeedCustomizationsTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedCustomizationsTaskHandler : INotificationHandler<SeedCustomizationsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedCustomizationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedCustomizationsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedCustomizationsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedCustomizationsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/customizations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<CustomizationPayload>? customizations = SeedingSerializer.Deserialize<IEnumerable<CustomizationPayload>>(json);
    if (customizations != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Customization.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Customization.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      foreach (CustomizationPayload customization in customizations)
      {
        string displayText = customization.DisplayName ?? customization.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = customization.UniqueSlug,
          DisplayName = customization.DisplayName,
          Description = customization.Description
        };
        CreateOrReplaceContentCommand command = new(customization.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for customization 'Id={customization.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for customization '{Customization}' (Id={Id}).", language.Locale, displayText, customization.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for customization '{Customization}' (Id={Id}).", language.Locale, displayText, customization.Id);
        }

        payload = new()
        {
          UniqueName = customization.UniqueSlug,
          DisplayName = customization.DisplayName
        };
        payload.AddFieldValue(fields[Customization.Type], customization.Type);
        command = new(customization.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for customization 'Id={customization.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for customization '{Customization}' (Id={Id}).", displayText, customization.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for customization '{Customization}' (Id={Id}).", displayText, customization.Id);
        }
      }
    }
  }
}
