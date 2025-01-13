using MediatR;
using SkillCraft.Tools.Core.Educations.Commands;
using SkillCraft.Tools.Core.Educations.Models;
using SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Tasks;

internal class SeedEducationsTask : SeedingTask
{
  public override string? Description => "Seeds the character educations.";
}

internal class SeedEducationsTaskHandler : INotificationHandler<SeedEducationsTask>
{
  private readonly ILogger<SeedEducationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedEducationsTaskHandler(ILogger<SeedEducationsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedEducationsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/educations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<EducationPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<EducationPayload>>(json);
    if (payloads != null)
    {
      foreach (EducationPayload payload in payloads)
      {
        CreateOrReplaceEducationCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceEducationResult result = await _mediator.Send(command, cancellationToken);
        EducationModel education = result.Education ?? throw new InvalidOperationException("The education model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The education '{Name}' has been {Status} (Id={Id}).", education.DisplayName ?? education.UniqueSlug, status, education.Id);
      }
    }
  }
}
