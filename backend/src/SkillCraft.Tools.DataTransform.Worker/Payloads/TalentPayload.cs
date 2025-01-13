using SkillCraft.Tools.Core.Talents.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record TalentPayload : CreateOrReplaceTalentPayload
{
  public Guid Id { get; set; }
}
