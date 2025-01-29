using SkillCraft.Tools.Core.Educations;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class WealthMultiplierConverter : JsonConverter<WealthMultiplier>
{
  public override WealthMultiplier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return reader.TryGetDouble(out double value) ? new WealthMultiplier(value) : null;
  }

  public override void Write(Utf8JsonWriter writer, WealthMultiplier wealthMultiplier, JsonSerializerOptions options)
  {
    writer.WriteNumberValue(wealthMultiplier.Value);
  }
}
