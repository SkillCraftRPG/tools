using SkillCraft.Tools.Core.Languages;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class ScriptConverter : JsonConverter<Script>
{
  public override Script? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return Script.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, Script script, JsonSerializerOptions options)
  {
    writer.WriteStringValue(script.Value);
  }
}
