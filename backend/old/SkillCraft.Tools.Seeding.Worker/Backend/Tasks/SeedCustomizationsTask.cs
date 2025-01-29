using MediatR;
using SkillCraft.Tools.Core.Customizations.Commands;
using SkillCraft.Tools.Core.Customizations.Models;
using SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Tasks;

internal class SeedCustomizationsTask : SeedingTask
{
  public override string? Description => "Seeds the character customizations.";
}

internal class SeedCustomizationsTaskHandler : INotificationHandler<SeedCustomizationsTask>
{
  private readonly ILogger<SeedCustomizationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedCustomizationsTaskHandler(ILogger<SeedCustomizationsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedCustomizationsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/customizations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<CustomizationPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<CustomizationPayload>>(json);
    if (payloads != null)
    {
      foreach (CustomizationPayload payload in payloads)
      {
        CreateOrReplaceCustomizationCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceCustomizationResult result = await _mediator.Send(command, cancellationToken);
        CustomizationModel customization = result.Customization ?? throw new InvalidOperationException("The customization model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The customization '{Name}' has been {Status} (Id={Id}).", customization.DisplayName ?? customization.UniqueSlug, status, customization.Id);
      }
    }
  }
}
