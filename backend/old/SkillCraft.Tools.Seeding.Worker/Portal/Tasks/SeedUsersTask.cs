using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Roles;
using Logitar.Portal.Contracts.Search;
using Logitar.Portal.Contracts.Users;
using MediatR;
using SkillCraft.Tools.Seeding.Worker.Portal.Payloads;

namespace SkillCraft.Tools.Seeding.Worker.Portal.Tasks;

internal class SeedUsersTask : SeedingTask
{
  public override string? Description => "Seeds the users into the Portal.";
}

internal class SeedUsersTaskHandler : INotificationHandler<SeedUsersTask>
{
  private readonly ILogger<SeedUsersTaskHandler> _logger;
  private readonly IUserClient _users;

  public SeedUsersTaskHandler(ILogger<SeedUsersTaskHandler> logger, IUserClient users)
  {
    _logger = logger;
    _users = users;
  }

  public async Task Handle(SeedUsersTask _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/data/users.json", Encoding.UTF8, cancellationToken);
    IEnumerable<UserPayload>? payloads = SeedingSerializer.Deserialize<IEnumerable<UserPayload>>(json);
    if (payloads != null)
    {
      SearchResults<UserModel> results = await _users.SearchAsync(new SearchUsersPayload(), context);
      Dictionary<Guid, UserModel> users = new(capacity: results.Items.Count);
      foreach (UserModel user in results.Items)
      {
        users[user.Id] = user;
      }

      foreach (UserPayload payload in payloads)
      {
        string status;
        if (users.TryGetValue(payload.Id, out UserModel? user))
        {
          UpdateUserPayload update = new()
          {
            UniqueName = payload.UniqueName,
            FirstName = new ChangeModel<string>(payload.FirstName),
            MiddleName = new ChangeModel<string>(payload.MiddleName),
            LastName = new ChangeModel<string>(payload.LastName),
            Picture = new ChangeModel<string>(payload.PictureUrl)
          };
          if (!string.IsNullOrWhiteSpace(payload.EmailAddress))
          {
            update.Email = new ChangeModel<EmailPayload>(new EmailPayload(payload.EmailAddress, isVerified: false));
          }
          foreach (string role in payload.Roles)
          {
            update.Roles.Add(new RoleModification(role, CollectionAction.Add));
          }
          user = await _users.UpdateAsync(user.Id, update, context) ?? throw new InvalidOperationException("The updated user should not be null.");
          status = "updated";
        }
        else
        {
          CreateUserPayload create = new()
          {
            Id = payload.Id,
            UniqueName = payload.UniqueName,
            Password = payload.Password,
            FirstName = payload.FirstName,
            MiddleName = payload.MiddleName,
            LastName = payload.LastName,
            Picture = payload.PictureUrl
          };
          if (!string.IsNullOrWhiteSpace(payload.EmailAddress))
          {
            create.Email = new EmailPayload(payload.EmailAddress, isVerified: false);
          }
          create.Roles.AddRange(payload.Roles);
          user = await _users.CreateAsync(create, context);
          users[user.Id] = user;
          status = "created";
        }

        _logger.LogInformation("The user '{Name}' has been {Status} (Id={Id}).", user.FullName ?? user.UniqueName, status, user.Id);
      }
    }
  }
}
