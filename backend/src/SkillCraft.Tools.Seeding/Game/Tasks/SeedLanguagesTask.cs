using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedLanguagesTask : SeedingTask
{
  public override string? Description => "Seeds the languages into the CMS.";

  public LanguageModel Language { get; }

  public SeedLanguagesTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedLanguagesTaskHandler : INotificationHandler<SeedLanguagesTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedLanguagesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedLanguagesTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedLanguagesTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedLanguagesTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/languages.json", Encoding.UTF8, cancellationToken);
    IEnumerable<LanguagePayload>? languages = SeedingSerializer.Deserialize<IEnumerable<LanguagePayload>>(json);
    if (languages != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Language.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Language.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      IReadOnlyDictionary<string, Guid> scripts = await LoadScriptsAsync(cancellationToken);

      foreach (LanguagePayload input in languages)
      {
        string displayText = input.DisplayName ?? input.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = input.UniqueSlug,
          DisplayName = input.DisplayName,
          Description = input.Description
        };
        if (!string.IsNullOrWhiteSpace(input.TypicalSpeakers))
        {
          string[] tags = input.TypicalSpeakers.Split(',').Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => value.Trim()).ToArray();
          payload.AddFieldValue(fields[Language.TypicalSpeakers], JsonSerializer.Serialize(tags));
        }
        CreateOrReplaceContentCommand command = new(input.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for language 'Id={input.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for language '{Language}' (Id={Id}).", language.Locale, displayText, input.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for language '{Language}' (Id={Id}).", language.Locale, displayText, input.Id);
        }

        payload = new()
        {
          UniqueName = input.UniqueSlug
        };
        if (!string.IsNullOrWhiteSpace(input.Script))
        {
          Guid[] contentIds = input.Script.Split(',').Where(value => !string.IsNullOrWhiteSpace(value)).Select(value => scripts[value.Trim()]).ToArray();
          payload.AddFieldValue(fields[Language.Scripts], JsonSerializer.Serialize(contentIds));
        }
        command = new(input.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for language 'Id={input.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for language '{Language}' (Id={Id}).", displayText, input.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for language '{Language}' (Id={Id}).", displayText, input.Id);
        }
      }
    }
  }

  private static async Task<IReadOnlyDictionary<string, Guid>> LoadScriptsAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/scripts.json", Encoding.UTF8, cancellationToken);
    IEnumerable<ScriptPayload>? scripts = SeedingSerializer.Deserialize<IEnumerable<ScriptPayload>>(json);

    Dictionary<string, Guid> results = [];
    if (scripts != null)
    {
      foreach (ScriptPayload script in scripts)
      {
        results[script.DisplayName ?? script.UniqueSlug] = script.Id;
      }
    }
    return results.AsReadOnly();
  }
}
