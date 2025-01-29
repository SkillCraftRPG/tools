using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedTalentsTask : SeedingTask
{
  public override string? Description => "Seeds the talents into the CMS.";

  public LanguageModel Language { get; }

  public SeedTalentsTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedTalentsTaskHandler : INotificationHandler<SeedTalentsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedTalentsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedTalentsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedTalentsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedTalentsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/talents.json", Encoding.UTF8, cancellationToken);
    IEnumerable<TalentPayload>? talents = SeedingSerializer.Deserialize<IEnumerable<TalentPayload>>(json);
    if (talents != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Talent.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Talent.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      Dictionary<string, Guid> talentIdByNames = [];
      foreach (TalentPayload talent in talents)
      {
        string displayText = talent.DisplayName ?? talent.UniqueSlug;
        talentIdByNames[displayText] = talent.Id;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = talent.UniqueSlug,
          DisplayName = talent.DisplayName,
          Description = talent.Description
        };
        CreateOrReplaceContentCommand command = new(talent.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for talent 'Id={talent.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for talent '{Talent}' (Id={Id}).", language.Locale, displayText, talent.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for talent '{Talent}' (Id={Id}).", language.Locale, displayText, talent.Id);
        }

        payload = new()
        {
          UniqueName = talent.UniqueSlug,
          DisplayName = talent.DisplayName
        };
        AddFieldValues(payload, fields, talent, talentIdByNames);
        command = new(talent.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for talent 'Id={talent.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for talent '{Talent}' (Id={Id}).", displayText, talent.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for talent '{Talent}' (Id={Id}).", displayText, talent.Id);
        }
      }
    }
  }

  private static void AddFieldValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, TalentPayload talent, Dictionary<string, Guid> talents)
  {
    payload.AddFieldValue(fields[Talent.Tier], talent.Tier);
    payload.AddFieldValue(fields[Talent.AllowMultiplePurchases], talent.AllowMultiplePurchases);
    if (talent.Skill.HasValue)
    {
      payload.AddFieldValue(fields[Talent.Skill], talent.Skill.Value);
    }
    if (!string.IsNullOrWhiteSpace(talent.RequiredTalent))
    {
      Guid contentId = talents[talent.RequiredTalent.Trim()];
      payload.AddFieldValue(fields[Talent.RequiredTalent], contentId);
    }
  }
}
