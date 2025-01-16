using SkillCraft.Tools.Core.Talents;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class TalentIdConverter : JsonConverter<TalentId>
{
  public override TalentId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? new TalentId() : new(value);
  }

  public override TalentId ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return Read(ref reader, typeToConvert, options);
  }

  public override void Write(Utf8JsonWriter writer, TalentId talentId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(talentId.Value);
  }

  public override void WriteAsPropertyName(Utf8JsonWriter writer, TalentId talentId, JsonSerializerOptions options)
  {
    writer.WritePropertyName(talentId.Value);
  }
}
