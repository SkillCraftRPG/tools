using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Lineages;

public readonly struct LineageId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public LineageId(Guid value)
  {
    StreamId = new(value);
  }
  public LineageId(string value)
  {
    StreamId = new(value);
  }
  public LineageId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static LineageId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(LineageId left, LineageId right) => left.Equals(right);
  public static bool operator !=(LineageId left, LineageId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is LineageId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
