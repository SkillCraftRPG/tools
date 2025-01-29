using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record NaturePayload : CreateOrReplaceNaturePayload
{
  public Guid Id { get; set; }
}
