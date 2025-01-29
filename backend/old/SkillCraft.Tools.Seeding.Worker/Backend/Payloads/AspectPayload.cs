using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record AspectPayload : CreateOrReplaceAspectPayload
{
  public Guid Id { get; set; }
}
