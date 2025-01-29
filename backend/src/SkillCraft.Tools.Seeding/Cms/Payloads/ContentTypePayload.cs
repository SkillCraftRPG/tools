using Logitar.Cms.Core.Contents.Models;

namespace SkillCraft.Tools.Seeding.Cms.Payloads;

internal record ContentTypePayload : CreateOrReplaceContentTypePayload
{
  public Guid Id { get; set; }
}
