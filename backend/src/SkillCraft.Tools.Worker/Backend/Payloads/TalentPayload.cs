using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.Worker.Backend.Payloads;

internal record TalentPayload : CreateOrReplaceTalentPayload
{
  public Guid Id { get; set; }
}
