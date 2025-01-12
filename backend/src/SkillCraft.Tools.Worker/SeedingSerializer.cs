﻿namespace SkillCraft.Tools.Worker;

internal static class SeedingSerializer
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedingSerializer()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _serializerOptions);
}
