using Logitar.EventSourcing;

namespace SkillCraft.Tools.Core.Customizations;

public readonly struct CustomizationId
{
  public StreamId StreamId { get; }
  public string Value => StreamId.Value;

  public CustomizationId(Guid value)
  {
    StreamId = new(value);
  }
  public CustomizationId(string value)
  {
    StreamId = new(value);
  }
  public CustomizationId(StreamId streamId)
  {
    StreamId = streamId;
  }

  public static CustomizationId NewId() => new(StreamId.NewId());

  public Guid ToGuid() => StreamId.ToGuid();

  public static bool operator ==(CustomizationId left, CustomizationId right) => left.Equals(right);
  public static bool operator !=(CustomizationId left, CustomizationId right) => !left.Equals(right);

  public override bool Equals([NotNullWhen(true)] object? obj) => obj is CustomizationId id && id.Value == Value;
  public override int GetHashCode() => Value.GetHashCode();
  public override string ToString() => Value;
}
