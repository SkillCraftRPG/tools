using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Talents;

public readonly struct TalentId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public TalentId(Guid value)
  {
    StreamId = new(value);
  }
  public TalentId(string value)
  {
    StreamId = new(value);
  }
  public TalentId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static TalentId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(TalentId left, TalentId right) => left.Equals(right);
  public static bool operator !=(TalentId left, TalentId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is TalentId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
