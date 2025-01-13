using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.Worker.Backend.Payloads;

internal record CastePayload : CreateOrReplaceCastePayload
{
  public Guid Id { get; set; }
}
