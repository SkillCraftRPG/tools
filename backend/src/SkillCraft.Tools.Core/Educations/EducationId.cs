using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Educations;

public readonly struct EducationId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public EducationId(Guid value)
  {
    StreamId = new(value);
  }
  public EducationId(string value)
  {
    StreamId = new(value);
  }
  public EducationId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static EducationId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(EducationId left, EducationId right) => left.Equals(right);
  public static bool operator !=(EducationId left, EducationId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is EducationId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
