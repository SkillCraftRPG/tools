using SkillCraft.Tools.Core.Aspects.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record AspectPayload : CreateOrReplaceAspectPayload
{
  public Guid Id { get; set; }
}
