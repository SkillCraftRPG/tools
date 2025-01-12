using Logitar.EventSourcing;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Worker;

internal class WorkerApplicationContext : IApplicationContext
{
  private readonly IConfiguration _configuration;

  public WorkerApplicationContext(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public ActorId? ActorId
  {
    get
    {
      Guid? actorId = _configuration.GetValue<Guid?>("ActorId");
      return actorId.HasValue ? new(actorId.Value) : null;
    }
  }
}
