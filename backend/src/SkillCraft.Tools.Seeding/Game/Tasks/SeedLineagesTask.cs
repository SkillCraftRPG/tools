using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedLineagesTask : SeedingTask
{
  public override string? Description => "Seeds the lineages and their traits into the CMS.";

  public LanguageModel Language { get; }

  public SeedLineagesTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedLineagesTaskHandler : INotificationHandler<SeedLineagesTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedLineagesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedLineagesTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedLineagesTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedLineagesTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/lineages.json", Encoding.UTF8, cancellationToken);
    IEnumerable<LineagePayload>? lineages = SeedingSerializer.Deserialize<IEnumerable<LineagePayload>>(json);
    if (lineages != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Lineage.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Lineage.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      IReadOnlyDictionary<string, Guid> languages = await LoadLanguagesAsync(cancellationToken);

      Dictionary<string, Guid> lineageIdByNames = [];
      foreach (LineagePayload lineage in lineages)
      {
        string displayText = lineage.DisplayName ?? lineage.UniqueSlug;
        lineageIdByNames[displayText] = lineage.Id;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = lineage.UniqueSlug,
          DisplayName = lineage.DisplayName,
          Description = lineage.Description
        };
        AddLocaleValues(payload, fields, lineage);
        CreateOrReplaceContentCommand command = new(lineage.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for lineage 'Id={lineage.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for lineage '{Lineage}' (Id={Id}).", language.Locale, displayText, lineage.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for lineage '{Lineage}' (Id={Id}).", language.Locale, displayText, lineage.Id);
        }

        payload = new()
        {
          UniqueName = lineage.UniqueSlug
        };
        AddInvariantValues(payload, fields, lineage, lineageIdByNames, languages);
        command = new(lineage.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for lineage 'Id={lineage.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for lineage '{Lineage}' (Id={Id}).", displayText, lineage.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for lineage '{Lineage}' (Id={Id}).", displayText, lineage.Id);
        }
      }
    }
  }

  private static void AddInvariantValues(
    CreateOrReplaceContentPayload payload,
    Dictionary<string, Guid> fields,
    LineagePayload lineage,
    Dictionary<string, Guid> lineages,
    IReadOnlyDictionary<string, Guid> languages)
  {
    if (!string.IsNullOrWhiteSpace(lineage.Parent))
    {
      Guid contentId = lineages[lineage.Parent.Trim()];
      payload.AddFieldValue(fields[Lineage.Parent], contentId);
    }

    payload.AddFieldValue(fields[Lineage.Agility], lineage.Attributes.Agility);
    payload.AddFieldValue(fields[Lineage.Coordination], lineage.Attributes.Coordination);
    payload.AddFieldValue(fields[Lineage.Intellect], lineage.Attributes.Intellect);
    payload.AddFieldValue(fields[Lineage.Presence], lineage.Attributes.Presence);
    payload.AddFieldValue(fields[Lineage.Sensitivity], lineage.Attributes.Sensitivity);
    payload.AddFieldValue(fields[Lineage.Spirit], lineage.Attributes.Spirit);
    payload.AddFieldValue(fields[Lineage.Vigor], lineage.Attributes.Vigor);
    payload.AddFieldValue(fields[Lineage.ExtraAttributes], lineage.Attributes.Extra);

    IEnumerable<Guid> traitIds = []; // TODO(fpion): implement
    payload.AddFieldValue(fields[Lineage.Traits], JsonSerializer.Serialize(traitIds));

    IEnumerable<Guid> languageIds = lineage.Languages.Items
      .Where(language => !string.IsNullOrWhiteSpace(language))
      .Select(language => languages[language.Trim()])
      .Distinct();
    payload.AddFieldValue(fields[Lineage.Languages], JsonSerializer.Serialize(languageIds));
    payload.AddFieldValue(fields[Lineage.ExtraLanguages], lineage.Languages.Extra);

    payload.AddFieldValue(fields[Lineage.FamilyNames], JsonSerializer.Serialize(lineage.Names.Family));
    payload.AddFieldValue(fields[Lineage.FemaleNames], JsonSerializer.Serialize(lineage.Names.Female));
    payload.AddFieldValue(fields[Lineage.MaleNames], JsonSerializer.Serialize(lineage.Names.Male));
    payload.AddFieldValue(fields[Lineage.UnisexNames], JsonSerializer.Serialize(lineage.Names.Unisex));
    if (lineage.Names.Custom.Count > 0)
    {
      payload.AddFieldValue(fields[Lineage.CustomNames], JsonSerializer.Serialize(lineage.Names.Custom));
    }

    payload.AddFieldValue(fields[Lineage.WalkSpeed], lineage.Speeds.Walk);
    payload.AddFieldValue(fields[Lineage.ClimbSpeed], lineage.Speeds.Climb);
    payload.AddFieldValue(fields[Lineage.SwimSpeed], lineage.Speeds.Swim);
    payload.AddFieldValue(fields[Lineage.FlySpeed], lineage.Speeds.Fly);
    payload.AddFieldValue(fields[Lineage.HoverSpeed], lineage.Speeds.Hover);
    payload.AddFieldValue(fields[Lineage.BurrowSpeed], lineage.Speeds.Burrow);

    payload.AddFieldValue(fields[Lineage.SizeCategory], lineage.Size.Category);
    if (!string.IsNullOrWhiteSpace(lineage.Size.Roll))
    {
      payload.AddFieldValue(fields[Lineage.SizeRoll], lineage.Size.Roll);
    }

    if (!string.IsNullOrWhiteSpace(lineage.Weight.Starved))
    {
      payload.AddFieldValue(fields[Lineage.StarvedRoll], lineage.Weight.Starved);
    }
    if (!string.IsNullOrWhiteSpace(lineage.Weight.Skinny))
    {
      payload.AddFieldValue(fields[Lineage.SkinnyRoll], lineage.Weight.Skinny);
    }
    if (!string.IsNullOrWhiteSpace(lineage.Weight.Normal))
    {
      payload.AddFieldValue(fields[Lineage.NormalRoll], lineage.Weight.Normal);
    }
    if (!string.IsNullOrWhiteSpace(lineage.Weight.Overweight))
    {
      payload.AddFieldValue(fields[Lineage.OverweightRoll], lineage.Weight.Overweight);
    }
    if (!string.IsNullOrWhiteSpace(lineage.Weight.Obese))
    {
      payload.AddFieldValue(fields[Lineage.ObeseRoll], lineage.Weight.Obese);
    }

    if (lineage.Ages.Adolescent.HasValue)
    {
      payload.AddFieldValue(fields[Lineage.Adolescent], lineage.Ages.Adolescent.Value);
    }
    if (lineage.Ages.Adult.HasValue)
    {
      payload.AddFieldValue(fields[Lineage.Adult], lineage.Ages.Adult.Value);
    }
    if (lineage.Ages.Mature.HasValue)
    {
      payload.AddFieldValue(fields[Lineage.Mature], lineage.Ages.Mature.Value);
    }
    if (lineage.Ages.Venerable.HasValue)
    {
      payload.AddFieldValue(fields[Lineage.Venerable], lineage.Ages.Venerable.Value);
    }
  }

  private static void AddLocaleValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, LineagePayload lineage)
  {
    if (!string.IsNullOrWhiteSpace(lineage.Languages.Text))
    {
      payload.AddFieldValue(fields[Lineage.LanguagesText], lineage.Languages.Text);
    }

    if (!string.IsNullOrWhiteSpace(lineage.Names.Text))
    {
      payload.AddFieldValue(fields[Lineage.NamesText], lineage.Names.Text);
    }
  }

  private static async Task<IReadOnlyDictionary<string, Guid>> LoadLanguagesAsync(CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/languages.json", Encoding.UTF8, cancellationToken);
    IEnumerable<LanguagePayload>? languages = SeedingSerializer.Deserialize<IEnumerable<LanguagePayload>>(json);

    Dictionary<string, Guid> results = [];
    if (languages != null)
    {
      foreach (LanguagePayload language in languages)
      {
        results[language.DisplayName ?? language.UniqueSlug] = language.Id;
      }
    }
    return results.AsReadOnly();
  }
}
