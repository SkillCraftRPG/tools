using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.Worker.Backend.Payloads;

internal record AspectPayload : CreateOrReplaceAspectPayload
{
  public Guid Id { get; set; }
}
