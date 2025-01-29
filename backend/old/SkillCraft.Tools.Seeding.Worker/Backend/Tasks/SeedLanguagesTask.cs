using MediatR;
using SkillCraft.Tools.Core.Languages.Commands;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Tasks;

internal class SeedLanguagesTask : SeedingTask
{
  public override string? Description => "Seeds the character languages.";
}

internal class SeedLanguagesTaskHandler : INotificationHandler<SeedLanguagesTask>
{
  private readonly ILogger<SeedLanguagesTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedLanguagesTaskHandler(ILogger<SeedLanguagesTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedLanguagesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/languages.json", Encoding.UTF8, cancellationToken);
    IEnumerable<LanguagePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<LanguagePayload>>(json);
    if (payloads != null)
    {
      foreach (LanguagePayload payload in payloads)
      {
        CreateOrReplaceLanguageCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceLanguageResult result = await _mediator.Send(command, cancellationToken);
        LanguageModel language = result.Language ?? throw new InvalidOperationException("The language model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The language '{Name}' has been {Status} (Id={Id}).", language.DisplayName ?? language.UniqueSlug, status, language.Id);
      }
    }
  }
}
