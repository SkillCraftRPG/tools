using SkillCraft.Tools.Core.Specializations;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class OtherOptionConverter : JsonConverter<OtherOption>
{
  public override OtherOption? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return OtherOption.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, OtherOption otherOption, JsonSerializerOptions options)
  {
    writer.WriteStringValue(otherOption.Value);
  }
}
