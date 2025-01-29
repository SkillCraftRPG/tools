using SkillCraft.Tools.Core.Castes.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record CastePayload : CreateOrReplaceCastePayload
{
  public Guid Id { get; set; }
}
