using MediatR;
using SkillCraft.Tools.Core.Lineages.Commands;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Tasks;

internal class SeedLineagesTask : SeedingTask
{
  public override string? Description => "Seeds the character lineages.";
}

internal class SeedLineagesTaskHandler : INotificationHandler<SeedLineagesTask>
{
  private readonly ILogger<SeedLineagesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedLineagesTaskHandler(ILogger<SeedLineagesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedLineagesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/lineages.json", Encoding.UTF8, cancellationToken);
    IEnumerable<LineagePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<LineagePayload>>(json);
    if (payloads != null)
    {
      foreach (LineagePayload payload in payloads)
      {
        CreateOrReplaceLineageCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceLineageResult result = await _mediator.Send(command, cancellationToken);
        LineageModel lineage = result.Lineage ?? throw new InvalidOperationException("The lineage model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The lineage '{Name}' has been {Status} (Id={Id}).", lineage.DisplayName ?? lineage.UniqueSlug, status, lineage.Id);
      }
    }
  }
}
