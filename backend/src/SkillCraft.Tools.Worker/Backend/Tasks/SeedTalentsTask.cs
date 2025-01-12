using MediatR;
using SkillCraft.Tools.Core.Talents.Commands;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Worker.Backend.Tasks;

internal class SeedTalentsTask : SeedingTask
{
  public override string? Description => "Seeds the character talents.";
}

internal class SeedTalentsTaskHandler : INotificationHandler<SeedTalentsTask>
{
  private readonly ILogger<SeedTalentsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedTalentsTaskHandler(ILogger<SeedTalentsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedTalentsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/talents.json", Encoding.UTF8, cancellationToken);
    IEnumerable<TalentPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<TalentPayload>>(json);
    if (payloads != null)
    {
      foreach (TalentPayload payload in payloads)
      {
        CreateOrReplaceTalentCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceTalentResult result = await _mediator.Send(command, cancellationToken);
        TalentModel talent = result.Talent ?? throw new InvalidOperationException("The talent model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The talent '{Name}' has been {Status} (Id={Id}).", talent.DisplayName ?? talent.UniqueSlug, status, talent.Id);
      }
    }
  }
}
