using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Cms;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedSpecializationsTask : SeedingTask
{
  public override string? Description => "Seeds the specializations into the CMS.";

  public LanguageModel Language { get; }
  public PublicationAction PublicationAction { get; }

  public SeedSpecializationsTask(LanguageModel language, PublicationAction publicationAction)
  {
    Language = language;
    PublicationAction = publicationAction;
  }
}

internal class SeedSpecializationsTaskHandler : INotificationHandler<SeedSpecializationsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedSpecializationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedSpecializationsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedSpecializationsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedSpecializationsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/specializations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<SpecializationPayload>? specializations = SeedingSerializer.Deserialize<IEnumerable<SpecializationPayload>>(json);
    if (specializations != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Specialization.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Specialization.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      IReadOnlyDictionary<string, Guid> talents = await LoadTalentsAsync(cancellationToken);

      foreach (SpecializationPayload specialization in specializations)
      {
        string displayText = specialization.DisplayName ?? specialization.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = specialization.UniqueSlug,
          DisplayName = specialization.DisplayName,
          Description = specialization.Description
        };
        AddLocaleValues(payload, fields, specialization);
        CreateOrReplaceContentCommand command = new(specialization.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for specialization 'Id={specialization.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for specialization '{Specialization}' (Id={Id}).", language.Locale, displayText, specialization.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for specialization '{Specialization}' (Id={Id}).", language.Locale, displayText, specialization.Id);
        }

        payload = new()
        {
          UniqueName = specialization.UniqueSlug,
          DisplayName = specialization.DisplayName
        };
        AddInvariantValues(payload, fields, specialization, talents);
        command = new(specialization.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for specialization 'Id={specialization.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for specialization '{Specialization}' (Id={Id}).", displayText, specialization.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for specialization '{Specialization}' (Id={Id}).", displayText, specialization.Id);
        }

        switch (task.PublicationAction)
        {
          case PublicationAction.Publish:
            await _mediator.Send(new PublishContentCommand(specialization.Id), cancellationToken);
            _logger.LogInformation("The contents were published for specialization '{Specialization}' (Id={Id}).", displayText, specialization.Id);
            break;
          case PublicationAction.Unpublish:
            await _mediator.Send(new UnpublishContentCommand(specialization.Id), cancellationToken);
            _logger.LogInformation("The contents were unpublished for specialization '{Specialization}' (Id={Id}).", displayText, specialization.Id);
            break;
        }
      }
    }
  }

  private static void AddInvariantValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, SpecializationPayload specialization, IReadOnlyDictionary<string, Guid> talents)
  {
    payload.AddFieldValue(fields[Specialization.Tier], specialization.Tier);

    if (!string.IsNullOrWhiteSpace(specialization.RequiredTalent))
    {
      Guid contentId = talents[specialization.RequiredTalent.Trim()];
      payload.AddFieldValue(fields[Specialization.RequiredTalent], contentId);
    }

    IEnumerable<Guid> contentIds = specialization.OptionalTalents
      .Where(talent => !string.IsNullOrWhiteSpace(talent))
      .Select(talent => talents[talent.Trim()])
      .Distinct();
    payload.AddFieldValue(fields[Specialization.OptionalTalents], JsonSerializer.Serialize(contentIds));
  }

  private static void AddLocaleValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, SpecializationPayload specialization)
  {
    IEnumerable<string> otherRequirements = specialization.OtherRequirements
      .Where(value => !string.IsNullOrWhiteSpace(value))
      .Select(value => value.Trim())
      .Distinct();
    if (otherRequirements.Any())
    {
      payload.AddFieldValue(fields[Specialization.OtherRequirements], string.Join('\n', otherRequirements));
    }

    IEnumerable<string> otherOptions = specialization.OtherOptions
      .Where(value => !string.IsNullOrWhiteSpace(value))
      .Select(value => value.Trim())
      .Distinct();
    if (otherOptions.Any())
    {
      payload.AddFieldValue(fields[Specialization.OtherOptions], string.Join('\n', otherOptions));
    }

    if (specialization.ReservedTalent != null)
    {
      if (!string.IsNullOrWhiteSpace(specialization.ReservedTalent.Name))
      {
        payload.AddFieldValue(fields[Specialization.ReservedTalentName], specialization.ReservedTalent.Name);
      }

      IEnumerable<string> descriptions = specialization.ReservedTalent.Descriptions
        .Where(description => !string.IsNullOrWhiteSpace(description))
        .Select(description => string.Concat("- ", description.Trim()))
        .Distinct();
      if (descriptions.Any())
      {
        string description = string.Join('\n', new string[] { "Le personnage acquiert les capacités suivantes.", string.Empty }.Concat(descriptions));
        payload.AddFieldValue(fields[Specialization.ReservedTalentDescription], description);
      }
    }
  }

  private static async Task<IReadOnlyDictionary<string, Guid>> LoadTalentsAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/talents.json", Encoding.UTF8, cancellationToken);
    IEnumerable<TalentPayload>? talents = SeedingSerializer.Deserialize<IEnumerable<TalentPayload>>(json);

    Dictionary<string, Guid> results = [];
    if (talents != null)
    {
      foreach (TalentPayload talent in talents)
      {
        results[talent.DisplayName ?? talent.UniqueSlug] = talent.Id;
      }
    }
    return results.AsReadOnly();
  }
}
