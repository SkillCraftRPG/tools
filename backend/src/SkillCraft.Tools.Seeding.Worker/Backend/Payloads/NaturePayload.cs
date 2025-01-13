using SkillCraft.Tools.Core.Natures.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record NaturePayload : CreateOrReplaceNaturePayload
{
  public Guid Id { get; set; }
}
