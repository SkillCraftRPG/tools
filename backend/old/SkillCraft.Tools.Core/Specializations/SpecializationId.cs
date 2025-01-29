using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Specializations;

public readonly struct SpecializationId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public SpecializationId(Guid value)
  {
    StreamId = new(value);
  }
  public SpecializationId(string value)
  {
    StreamId = new(value);
  }
  public SpecializationId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static SpecializationId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(SpecializationId left, SpecializationId right) => left.Equals(right);
  public static bool operator !=(SpecializationId left, SpecializationId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is SpecializationId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
