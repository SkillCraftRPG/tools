using SkillCraft.Tools.Core.Languages;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class TypicalSpeakersConverter : JsonConverter<TypicalSpeakers>
{
  public override TypicalSpeakers? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return TypicalSpeakers.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, TypicalSpeakers typicalSpeakers, JsonSerializerOptions options)
  {
    writer.WriteStringValue(typicalSpeakers.Value);
  }
}
