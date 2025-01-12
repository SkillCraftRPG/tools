using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Realms;
using MediatR;
using SkillCraft.Tools.Worker.Portal.Payloads;

namespace SkillCraft.Tools.Worker.Portal.Tasks;

internal class SeedRealmTask : SeedingTask
{
  public override string? Description => "Seeds the Realm into the Portal.";
}

internal class SeedRealmTaskHandler : INotificationHandler<SeedRealmTask>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedRealmTaskHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  } // TODO(fpion): refactor

  private readonly ILogger<SeedRealmTaskHandler> _logger;
  private readonly IRealmClient _realms;

  public SeedRealmTaskHandler(IConfiguration configuration, ILogger<SeedRealmTaskHandler> logger, IRealmClient realms)
  {
    _logger = logger;
    _realms = realms;
  }

  public async Task Handle(SeedRealmTask _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string json = await File.ReadAllTextAsync("Portal/data/realm.json", Encoding.UTF8, cancellationToken);
    RealmPayload? payload = JsonSerializer.Deserialize<RealmPayload>(json, _serializerOptions);
    if (payload == null)
    {
      _logger.LogWarning("The realm could not be deserialized.");
    }
    else
    {
      RealmModel? realm = await _realms.ReadAsync(payload.Id, uniqueSlug: null, context);
      string status;
      if (realm == null)
      {
        CreateRealmPayload create = new()
        {
          Id = payload.Id,
          UniqueSlug = payload.UniqueSlug,
          DisplayName = payload.DisplayName,
          Description = payload.Description,
          DefaultLocale = payload.DefaultLocale,
          Url = payload.Url
        };
        realm = await _realms.CreateAsync(create, context);
        status = "created";
      }
      else
      {
        UpdateRealmPayload update = new()
        {
          UniqueSlug = payload.UniqueSlug,
          DisplayName = new ChangeModel<string>(payload.DisplayName),
          Description = new ChangeModel<string>(payload.Description),
          DefaultLocale = new ChangeModel<string>(payload.DefaultLocale),
          Url = new ChangeModel<string>(payload.Url)
        };
        realm = await _realms.UpdateAsync(realm.Id, update, context) ?? throw new InvalidOperationException("The updated realm should not be null.");
        status = "updated";
      }

      WorkerPortalSettings.Instance.SetRealm(realm);

      _logger.LogInformation("The realm '{Name}' has been {Status} (Id={Id}).", realm.DisplayName ?? realm.UniqueSlug, status, realm.Id);
    }
  }
}
