using MediatR;
using SkillCraft.Tools.Core.Natures.Commands;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Worker.Backend.Tasks;

internal class SeedNaturesTask : SeedingTask
{
  public override string? Description => "Seeds the character natures.";
}

internal class SeedNaturesTaskHandler : INotificationHandler<SeedNaturesTask>
{
  private readonly ILogger<SeedNaturesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedNaturesTaskHandler(ILogger<SeedNaturesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedNaturesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/natures.json", Encoding.UTF8, cancellationToken);
    IEnumerable<NaturePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<NaturePayload>>(json);
    if (payloads != null)
    {
      foreach (NaturePayload payload in payloads)
      {
        CreateOrReplaceNatureCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceNatureResult result = await _mediator.Send(command, cancellationToken);
        NatureModel nature = result.Nature ?? throw new InvalidOperationException("The nature model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The nature '{Name}' has been {Status} (Id={Id}).", nature.DisplayName ?? nature.UniqueSlug, status, nature.Id);
      }
    }
  }
}
