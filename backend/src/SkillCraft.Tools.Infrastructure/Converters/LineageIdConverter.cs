﻿using SkillCraft.Tools.Core.Lineages;

namespace SkillCraft.Tools.Infrastructure.Converters;

internal class LineageIdConverter : JsonConverter<LineageId>
{
  public override LineageId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    return string.IsNullOrWhiteSpace(value) ? new LineageId() : new(value);
  }

  public override void Write(Utf8JsonWriter writer, LineageId lineageId, JsonSerializerOptions options)
  {
    writer.WriteStringValue(lineageId.Value);
  }
}
