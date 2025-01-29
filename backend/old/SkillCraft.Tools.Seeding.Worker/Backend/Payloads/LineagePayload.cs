using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record LineagePayload : CreateOrReplaceLineagePayload
{
  public Guid Id { get; set; }
}
