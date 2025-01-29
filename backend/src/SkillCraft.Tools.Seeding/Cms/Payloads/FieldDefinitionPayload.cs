using Logitar.Cms.Core.Fields.Models;

namespace SkillCraft.Tools.Seeding.Cms.Payloads;

internal record FieldDefinitionPayload : CreateOrReplaceFieldDefinitionPayload
{
  public Guid ContentTypeId { get; set; }
  public Guid FieldDefinitionId { get; set; }
}
