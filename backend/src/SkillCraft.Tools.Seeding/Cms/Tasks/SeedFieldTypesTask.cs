using Logitar.Cms.Core.Fields.Commands;
using MediatR;
using SkillCraft.Tools.Seeding.Cms.Payloads;

namespace SkillCraft.Tools.Seeding.Cms.Tasks;

internal class SeedFieldTypesTask : SeedingTask
{
  public override string? Description => "Seeds the field types into the CMS.";
}

internal class SeedFieldTypesTaskHandler : INotificationHandler<SeedFieldTypesTask>
{
  private readonly ILogger<SeedFieldTypesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedFieldTypesTaskHandler(ILogger<SeedFieldTypesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedFieldTypesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Cms/data/field_types.json", Encoding.UTF8, cancellationToken);
    IEnumerable<FieldTypePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<FieldTypePayload>>(json);
    if (payloads != null)
    {
      foreach (FieldTypePayload payload in payloads)
      {
        CreateOrReplaceFieldTypeCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceFieldTypeResult result = await _mediator.Send(command, cancellationToken);
        if (result.FieldType == null)
        {
          throw new InvalidOperationException($"'{nameof(CreateOrReplaceFieldTypeCommand)}' returned null for field type 'Id={command.Id}'.");
        }
        else if (result.Created)
        {
          _logger.LogInformation("The field type '{FieldType}' was created.", result.FieldType.DisplayName ?? result.FieldType.UniqueName);
        }
        else
        {
          _logger.LogInformation("The field type '{FieldType}' was updated.", result.FieldType.DisplayName ?? result.FieldType.UniqueName);
        }
      }
    }
  }
}
