using SkillCraft.Tools.Core.Specializations;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class OtherRequirementConverter : JsonConverter<OtherRequirement>
{
  public override OtherRequirement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    return OtherRequirement.TryCreate(reader.GetString());
  }

  public override void Write(Utf8JsonWriter writer, OtherRequirement otherRequirement, JsonSerializerOptions options)
  {
    writer.WriteStringValue(otherRequirement.Value);
  }
}
