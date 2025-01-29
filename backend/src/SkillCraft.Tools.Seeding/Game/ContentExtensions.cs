using Logitar.Cms.Core.Contents.Models;
using Logitar.Cms.Core.Fields.Models;

namespace SkillCraft.Tools.Seeding.Game;

internal static class ContentExtensions
{
  public static void AddFieldValue(this CreateOrReplaceContentPayload payload, Guid id, object? value)
  {
    FieldValue fieldValue = new(id, value?.ToString() ?? string.Empty);
    payload.FieldValues.Add(fieldValue);
  }
}
