using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Search;
using MediatR;
using SkillCraft.Tools.Seeding.Worker.Portal.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Portal.Tasks;

internal class SeedRolesTask : SeedingTask
{
  public override string? Description => "Seeds the roles into the Portal.";
}

internal class SeedRolesTaskHandler : INotificationHandler<SeedRolesTask>
{
  private readonly ILogger<SeedRolesTaskHandler> _logger;
  private readonly IRoleClient _roles;

  public SeedRolesTaskHandler(ILogger<SeedRolesTaskHandler> logger, IRoleClient roles)
  {
    _logger = logger;
    _roles = roles;
  }

  public async Task Handle(SeedRolesTask _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/data/roles.json", Encoding.UTF8, cancellationToken);
    IEnumerable<RolePayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<RolePayload>>(json);
    if (payloads != null)
    {
      SearchResults<RoleModel> results = await _roles.SearchAsync(new SearchRolesPayload(), context);
      Dictionary<Guid, RoleModel> roles = new(capacity: results.Items.Count);
      foreach (RoleModel role in results.Items)
      {
        roles[role.Id] = role;
      }

      foreach (RolePayload payload in payloads)
      {
        string status;
        if (roles.TryGetValue(payload.Id, out RoleModel? role))
        {
          UpdateRolePayload update = new()
          {
            UniqueName = payload.UniqueName,
            DisplayName = new ChangeModel<string>(payload.DisplayName),
            Description = new ChangeModel<string>(payload.Description)
          };
          role = await _roles.UpdateAsync(role.Id, update, context) ?? throw new InvalidOperationException("The updated role should not be null.");
          status = "updated";
        }
        else
        {
          CreateRolePayload create = new()
          {
            Id = payload.Id,
            UniqueName = payload.UniqueName,
            DisplayName = payload.DisplayName,
            Description = payload.Description
          };
          role = await _roles.CreateAsync(create, context);
          roles[role.Id] = role;
          status = "created";
        }

        _logger.LogInformation("The role '{Name}' has been {Status} (Id={Id}).", role.DisplayName ?? role.UniqueName, status, role.Id);
      }
    }
  }
}
