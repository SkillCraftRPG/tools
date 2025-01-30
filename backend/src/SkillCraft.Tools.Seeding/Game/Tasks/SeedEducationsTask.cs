using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Cms;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedEducationsTask : SeedingTask
{
  public override string? Description => "Seeds the educations into the CMS.";

  public LanguageModel Language { get; }
  public PublicationAction PublicationAction { get; }

  public SeedEducationsTask(LanguageModel language, PublicationAction publicationAction)
  {
    Language = language;
    PublicationAction = publicationAction;
  }
}

internal class SeedEducationsTaskHandler : INotificationHandler<SeedEducationsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedEducationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedEducationsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedEducationsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedEducationsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/educations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<EducationPayload>? educations = SeedingSerializer.Deserialize<IEnumerable<EducationPayload>>(json);
    if (educations != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Education.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Education.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      foreach (EducationPayload education in educations)
      {
        string displayText = education.DisplayName ?? education.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = education.UniqueSlug,
          DisplayName = education.DisplayName,
          Description = education.Description
        };
        CreateOrReplaceContentCommand command = new(education.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for education 'Id={education.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for education '{Education}' (Id={Id}).", language.Locale, displayText, education.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for education '{Education}' (Id={Id}).", language.Locale, displayText, education.Id);
        }

        payload = new()
        {
          UniqueName = education.UniqueSlug,
          DisplayName = education.DisplayName
        };
        AddFieldValues(payload, fields, education);
        command = new(education.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for education 'Id={education.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for education '{Education}' (Id={Id}).", displayText, education.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for education '{Education}' (Id={Id}).", displayText, education.Id);
        }

        switch (task.PublicationAction)
        {
          case PublicationAction.Publish:
            await _mediator.Send(new PublishContentCommand(education.Id), cancellationToken);
            _logger.LogInformation("The contents were published for education '{Education}' (Id={Id}).", displayText, education.Id);
            break;
          case PublicationAction.Unpublish:
            await _mediator.Send(new UnpublishContentCommand(education.Id), cancellationToken);
            _logger.LogInformation("The contents were unpublished for education '{Education}' (Id={Id}).", displayText, education.Id);
            break;
        }
      }
    }
  }

  private static void AddFieldValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, EducationPayload education)
  {
    if (education.Skill.HasValue)
    {
      payload.AddFieldValue(fields[Education.Skill], education.Skill.Value);
    }
    if (education.WealthMultiplier.HasValue)
    {
      payload.AddFieldValue(fields[Education.WealthMultiplier], education.WealthMultiplier.Value);
    }
  }
}
