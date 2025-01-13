using SkillCraft.Tools.Core.Aspects;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class AspectIdConverter : JsonConverter<AspectId>
{
  public override AspectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? new AspectId() : new(value);
  }

  public override void Write(Utf8JsonWriter writer, AspectId aspectId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(aspectId.Value);
  }
}
