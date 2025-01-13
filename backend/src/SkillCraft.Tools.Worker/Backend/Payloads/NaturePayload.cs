using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Worker.Backend.Payloads;

internal record NaturePayload : CreateOrReplaceNaturePayload
{
  public Guid Id { get; set; }
}
