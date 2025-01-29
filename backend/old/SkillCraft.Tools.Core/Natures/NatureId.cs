using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Natures;

public readonly struct NatureId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public NatureId(Guid value)
  {
    StreamId = new(value);
  }
  public NatureId(string value)
  {
    StreamId = new(value);
  }
  public NatureId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static NatureId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(NatureId left, NatureId right) => left.Equals(right);
  public static bool operator !=(NatureId left, NatureId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is NatureId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
