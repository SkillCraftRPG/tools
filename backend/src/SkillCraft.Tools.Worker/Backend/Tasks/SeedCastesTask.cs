using MediatR;
using SkillCraft.Tools.Core.Castes.Commands;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Worker.Backend.Tasks;

internal class SeedCastesTask : SeedingTask
{
  public override string? Description => "Seeds the character castes.";
}

internal class SeedCastesTaskHandler : INotificationHandler<SeedCastesTask>
{
  private readonly ILogger<SeedCastesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedCastesTaskHandler(ILogger<SeedCastesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedCastesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/castes.json", Encoding.UTF8, cancellationToken);
    IEnumerable<CastePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<CastePayload>>(json);
    if (payloads != null)
    {
      foreach (CastePayload payload in payloads)
      {
        CreateOrReplaceCasteCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceCasteResult result = await _mediator.Send(command, cancellationToken);
        CasteModel caste = result.Caste ?? throw new InvalidOperationException("The caste model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The caste '{Name}' has been {Status} (Id={Id}).", caste.DisplayName ?? caste.UniqueSlug, status, caste.Id);
      }
    }
  }
}
