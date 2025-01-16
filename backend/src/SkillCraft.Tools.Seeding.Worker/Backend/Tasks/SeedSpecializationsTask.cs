using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Core.Specializations.Commands;
using SkillCraft.Tools.Core.Specializations.Models;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Core.Talents.Queries;
using SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Tasks;

internal class SeedSpecializationsTask : SeedingTask
{
  public override string? Description => "Seeds the character specializations.";
}

internal class SeedSpecializationsTaskHandler : INotificationHandler<SeedSpecializationsTask>
{
  private readonly ILogger<SeedSpecializationsTaskHandler> _logger;
  private readonly IMediator _mediator;

  public SeedSpecializationsTaskHandler(ILogger<SeedSpecializationsTaskHandler> logger, IMediator mediator)
  {
    _logger = logger;
    _mediator = mediator;
  }

  public async Task Handle(SeedSpecializationsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/data/specializations.json", Encoding.UTF8, cancellationToken);
    IEnumerable<SpecializationInput>? inputs = SeedingSerializer.Deserialize<IEnumerable<SpecializationInput>>(json);
    if (inputs != null)
    {
      SearchTalentsQuery searchTalents = new(new SearchTalentsPayload());
      SearchResults<TalentModel> talents = await _mediator.Send(searchTalents, cancellationToken);
      Dictionary<string, Guid> talentIdByDisplayNames = talents.Items
        .Where(x => x.DisplayName != null)
        .ToDictionary(x => x.DisplayName!, x => x.Id);

      foreach (SpecializationInput input in inputs)
      {
        CreateOrReplaceSpecializationPayload payload = new()
        {
          Tier = input.Tier,
          UniqueSlug = input.UniqueSlug,
          DisplayName = input.DisplayName,
          Description = input.Description,
          ReservedTalent = input.ReservedTalent
        };
        payload.OtherRequirements.AddRange(input.OtherRequirements);
        payload.OtherOptions.AddRange(input.OtherOptions);

        if (input.RequiredTalent != null && talentIdByDisplayNames.TryGetValue(input.RequiredTalent, out Guid requiredTalentId))
        {
          payload.RequiredTalentId = requiredTalentId;
        }
        foreach (string optionalTalent in input.OptionalTalents)
        {
          if (talentIdByDisplayNames.TryGetValue(optionalTalent, out Guid optionalTalentId))
          {
            payload.OptionalTalentIds.Add(optionalTalentId);
          }
        }

        CreateOrReplaceSpecializationCommand command = new(input.Id, payload, Version: null);
        CreateOrReplaceSpecializationResult result = await _mediator.Send(command, cancellationToken);
        SpecializationModel specialization = result.Specialization ?? throw new InvalidOperationException("The specialization model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The specialization '{Name}' has been {Status} (Id={Id}).", specialization.DisplayName ?? specialization.UniqueSlug, status, specialization.Id);
      }
    }
  }
}
