using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Castes;

public readonly struct CasteId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public CasteId(Guid value)
  {
    StreamId = new(value);
  }
  public CasteId(string value)
  {
    StreamId = new(value);
  }
  public CasteId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static CasteId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(CasteId left, CasteId right) => left.Equals(right);
  public static bool operator !=(CasteId left, CasteId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is CasteId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
