using Logitar.Cms.Core.Contents.Commands;
using MediatR;
using SkillCraft.Tools.Seeding.Cms.Payloads;
using System.Text;

namespace SkillCraft.Tools.Seeding.Cms.Tasks;

internal class SeedContentTypesTask : SeedingTask
{
  public override string? Description => "Seeds the content types into the CMS.";
}

internal class SeedContentTypesTaskHandler : INotificationHandler<SeedContentTypesTask>
{
  private readonly ILogger<SeedContentTypesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedContentTypesTaskHandler(ILogger<SeedContentTypesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedContentTypesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Cms/data/content_types.json", Encoding.UTF8, cancellationToken);
    IEnumerable<ContentTypePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<ContentTypePayload>>(json);
    if (payloads != null)
    {
      foreach (ContentTypePayload payload in payloads)
      {
        CreateOrReplaceContentTypeCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceContentTypeResult result = await _mediator.Send(command, cancellationToken);
        if (result.ContentType == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceContentTypeCommand)}' returned null for content type 'Id={command.Id}'.");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The content type '{ContentType}' was created.", result.ContentType.DisplayName ?? result.ContentType.UniqueName);
        }
        else
        {
          _logger.LogInformation("The content type '{ContentType}' was updated.", result.ContentType.DisplayName ?? result.ContentType.UniqueName);
        }
      }
    }
  }
}
