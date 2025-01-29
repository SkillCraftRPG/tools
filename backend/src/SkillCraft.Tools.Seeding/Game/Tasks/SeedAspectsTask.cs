using Logitar.Cms.Core.Contents;
using Logitar.Cms.Core.Contents.Commands;
using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Localization.Models;
using MediatR;
using SkillCraft.Tools.Core.Contents;
using SkillCraft.Tools.Seeding.Game.Payloads;

namespace SkillCraft.Tools.Seeding.Game.Tasks;

internal class SeedAspectsTask : SeedingTask
{
  public override string? Description => "Seeds the aspects into the CMS.";

  public LanguageModel Language { get; }

  public SeedAspectsTask(LanguageModel language)
  {
    Language = language;
  }
}

internal class SeedAspectsTaskHandler : INotificationHandler<SeedAspectsTask>
{
  private readonly IContentTypeQuerier _contentTypeQuerier;
  private readonly ILogger<SeedAspectsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedAspectsTaskHandler(IContentTypeQuerier contentTypeQuerier, ILogger<SeedAspectsTaskHandler> logger, IMediator mediator)
  {
    _contentTypeQuerier = contentTypeQuerier;
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedAspectsTask task, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Game/data/aspects.json", Encoding.UTF8, cancellationToken);
    IEnumerable<AspectPayload>? aspects = SeedingSerializer.Deserialize<IEnumerable<AspectPayload>>(json);
    if (aspects != null)
    {
      LanguageModel language = task.Language;
      ContentTypeModel contentType = await _contentTypeQuerier.ReadAsync(Aspect.UniqueName, cancellationToken)
        ?? throw new InvalidOperationException($"The content type '{Aspect.UniqueName}' could not be found.");
      Dictionary<string, Guid> fields = contentType.Fields.ToDictionary(x => x.UniqueName, x => x.Id);

      foreach (AspectPayload aspect in aspects)
      {
        string displayText = aspect.DisplayName ?? aspect.UniqueSlug;

        CreateOrReplaceContentPayload payload = new()
        {
          ContentTypeId = contentType.Id,
          UniqueName = aspect.UniqueSlug,
          DisplayName = aspect.DisplayName,
          Description = aspect.Description
        };
        CreateOrReplaceContentCommand command = new(aspect.Id, language.Id, payload);
        CreateOrReplaceContentResult result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for aspect 'Id={aspect.Id}' (locale).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale ({Language}) was created for aspect '{Aspect}' (Id={Id}).", language.Locale, displayText, aspect.Id);
        }
        else
        {
          _logger.LogInformation("The content locale ({Language}) was updated for aspect '{Aspect}' (Id={Id}).", language.Locale, displayText, aspect.Id);
        }

        payload = new()
        {
          UniqueName = aspect.UniqueSlug,
          DisplayName = aspect.DisplayName
        };
        AddFieldValues(payload, fields, aspect);
        command = new(aspect.Id, LanguageId: null, payload);
        result = await _mediator.Send(command, cancellationToken);
        if (result.Content == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentCommand)}' returned null for aspect 'Id={aspect.Id}' (invariant).");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content locale invariant was created for aspect '{Aspect}' (Id={Id}).", displayText, aspect.Id);
        }
        else
        {
          _logger.LogInformation("The content locale invariant was updated for aspect '{Aspect}' (Id={Id}).", displayText, aspect.Id);
        }
      }
    }
  }

  private static void AddFieldValues(CreateOrReplaceContentPayload payload, Dictionary<string, Guid> fields, AspectPayload aspect)
  {
    if (aspect.Attributes.Mandatory1.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.MandatoryAttribute1], aspect.Attributes.Mandatory1.Value);
    }
    if (aspect.Attributes.Mandatory2.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.MandatoryAttribute2], aspect.Attributes.Mandatory2.Value);
    }
    if (aspect.Attributes.Optional1.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.OptionalAttribute1], aspect.Attributes.Optional1.Value);
    }
    if (aspect.Attributes.Optional2.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.OptionalAttribute2], aspect.Attributes.Optional2.Value);
    }
    if (aspect.Skills.Discounted1.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.DiscountedSkill1], aspect.Skills.Discounted1.Value);
    }
    if (aspect.Skills.Discounted2.HasValue)
    {
      payload.AddFieldValue(fields[Aspect.DiscountedSkill2], aspect.Skills.Discounted2.Value);
    }
  }
}
