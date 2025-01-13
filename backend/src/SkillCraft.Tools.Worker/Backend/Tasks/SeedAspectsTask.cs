using MediatR;
using SkillCraft.Tools.Core.Aspects.Commands;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Worker.Backend.Tasks;

internal class SeedAspectsTask : SeedingTask
{
  public override string? Description => "Seeds the character aspects.";
}

internal class SeedAspectsTaskHandler : INotificationHandler<SeedAspectsTask>
{
  private readonly ILogger<SeedAspectsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedAspectsTaskHandler(ILogger<SeedAspectsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedAspectsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/aspects.json", Encoding.UTF8, cancellationToken);
    IEnumerable<AspectPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<AspectPayload>>(json);
    if (payloads != null)
    {
      foreach (AspectPayload payload in payloads)
      {
        CreateOrReplaceAspectCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceAspectResult result = await _mediator.Send(command, cancellationToken);
        AspectModel aspect = result.Aspect ?? throw new InvalidOperationException("The aspect model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The aspect '{Name}' has been {Status} (Id={Id}).", aspect.DisplayName ?? aspect.UniqueSlug, status, aspect.Id);
      }
    }
  }
}
