using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Cms;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedScriptsTask : SeedingTask
{
  public override string? Description => "Seeds the scripts into the CMS.";

  public LanguageModel Language { get; }
  public PublicationAction PublicationAction { get; }

  public SeedScriptsTask(LanguageModel language, PublicationAction publicationAction)
  {
    Language = language;
    PublicationAction = publicationAction;
  }
}

internal class SeedScriptsTaskHandler : INotificationHandler<SeedScriptsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedScriptsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedScriptsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedScriptsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedScriptsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/scripts.json", Encoding.UTF8, cancellationToken);
    IEnumerable<ScriptPayload>? scripts = SeedingSerializer.Deserialize<IEnumerable<ScriptPayload>>(json);
    if (scripts != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Script.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Script.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      foreach (ScriptPayload script in scripts)
      {
        string displayText = script.DisplayName ?? script.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = script.UniqueSlug,
          DisplayName = script.DisplayName,
          Description = script.Description
        };
        CreateOrReplaceContentCommand command = new(script.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for script 'Id={script.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for script '{Script}' (Id={Id}).", language.Locale, displayText, script.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for script '{Script}' (Id={Id}).", language.Locale, displayText, script.Id);
        }

        payload = new()
        {
          UniqueName = script.UniqueSlug,
          DisplayName = script.DisplayName
        };
        command = new(script.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for script 'Id={script.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for script '{Script}' (Id={Id}).", displayText, script.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for script '{Script}' (Id={Id}).", displayText, script.Id);
        }

        switch (task.PublicationAction)
        {
          case PublicationAction.Publish:
            await _mediator.Send(new PublishContentCommand(script.Id), cancellationToken);
            _logger.LogInformation("The contents were published for script '{Script}' (Id={Id}).", displayText, script.Id);
            break;
          case PublicationAction.Unpublish:
            await _mediator.Send(new UnpublishContentCommand(script.Id), cancellationToken);
            _logger.LogInformation("The contents were unpublished for script '{Script}' (Id={Id}).", displayText, script.Id);
            break;
        }
      }
    }
  }
}
