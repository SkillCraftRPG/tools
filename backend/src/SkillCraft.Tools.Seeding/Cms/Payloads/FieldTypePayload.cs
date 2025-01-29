using Logitar.Cms.Core.Fields.Models;

namespace SkillCraft.Tools.Seeding.Cms.Payloads;

internal record FieldTypePayload : CreateOrReplaceFieldTypePayload
{
  public Guid Id { get; set; }
}
