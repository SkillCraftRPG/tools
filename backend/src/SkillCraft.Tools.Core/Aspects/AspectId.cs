using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Aspects;

public readonly struct AspectId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public AspectId(Guid value)
  {
    StreamId = new(value);
  }
  public AspectId(string value)
  {
    StreamId = new(value);
  }
  public AspectId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static AspectId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(AspectId left, AspectId right) => left.Equals(right);
  public static bool operator !=(AspectId left, AspectId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is AspectId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
