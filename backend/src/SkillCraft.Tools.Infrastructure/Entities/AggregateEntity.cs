using Logitar.EventSourcing;

namespace SkillCraft.Tools.Infrastructure.Entities;

internal abstract class AggregateEntity
{
  public string StreamId { get; private set; } = string.Empty;
  public long Version { get; private set; }

  public string? CreatedBy { get; private set; }
  public DateTime CreatedOn { get; private set; }

  public string? UpdatedBy { get; private set; }
  public DateTime UpdatedOn { get; private set; }

  protected AggregateEntity()
  {
  }

  public virtual IReadOnlyCollection<ActorId> GetActorIds()
  {
    List<ActorId> actorIds = new(capacity: 2);
    if (CreatedBy != null)
    {
      actorIds.Add(new ActorId(CreatedBy));
    }
    if (UpdatedBy != null)
    {
      actorIds.Add(new ActorId(UpdatedBy));
    }
    return actorIds.AsReadOnly();
  }

  public override bool Equals(object? obj) => obj is AggregateEntity aggregate && aggregate.StreamId == StreamId;
  public override int GetHashCode() => StreamId.GetHashCode();
  public override string ToString() => $"{GetType()} (StreamId={StreamId})";
}
