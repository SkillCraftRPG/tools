using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Fields.Commands;
using MediatR;
using SkillCraft.Tools.Seeding.Cms.Payloads;

namespace SkillCraft.Tools.Seeding.Cms.Tasks;

internal class SeedFieldDefinitionsTask : SeedingTask
{
  public override string? Description => "Seeds the field definitions into the CMS.";
}

internal class SeedFieldDefinitionsTaskHandler : INotificationHandler<SeedFieldDefinitionsTask>
{
  private readonly ILogger<SeedFieldDefinitionsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedFieldDefinitionsTaskHandler(ILogger<SeedFieldDefinitionsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedFieldDefinitionsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Cms/data/field_definitions.json", Encoding.UTF8, cancellationToken);
    IEnumerable<FieldDefinitionPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<FieldDefinitionPayload>>(json);
    if (payloads != null)
    {
      foreach (FieldDefinitionPayload payload in payloads)
      {
        CreateOrReplaceFieldDefinitionCommand command = new(payload.ContentTypeId, payload.FieldDefinitionId, payload);
        ContentTypeModel? contentType = await _mediator.Send(command, cancellationToken);
        if (contentType == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceFieldDefinitionCommand)}' returned null for field definition 'FieldId={command.FieldId}'.");
        }
        else
        {
          _logger.LogInformation("The field definition '{FieldDefinition}' was saved.", payload.DisplayName ?? payload.UniqueName);
        }
      }
    }
  }
}
