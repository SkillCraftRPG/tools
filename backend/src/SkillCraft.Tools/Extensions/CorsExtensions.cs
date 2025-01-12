﻿using SkillCraft.Tools.Settings;

namespace SkillCraft.Tools.Extensions;

internal static class CorsExtensions
{
  public static void UseCors(this IApplicationBuilder builder, CorsSettings settings)
  {
    builder.UseCors(cors =>
    {
      if (settings.AllowAnyOrigin)
      {
        cors.AllowAnyOrigin();
      }
      else
      {
        cors.WithOrigins(settings.AllowedOrigins);
      }

      if (settings.AllowAnyMethod)
      {
        cors.AllowAnyMethod();
      }
      else
      {
        cors.WithMethods(settings.AllowedMethods);
      }

      if (settings.AllowAnyHeader)
      {
        cors.AllowAnyHeader();
      }
      else
      {
        cors.WithHeaders(settings.AllowedHeaders);
      }

      if (settings.AllowCredentials)
      {
        cors.AllowCredentials();
      }
      else
      {
        cors.DisallowCredentials();
      }
    });
  }
}
