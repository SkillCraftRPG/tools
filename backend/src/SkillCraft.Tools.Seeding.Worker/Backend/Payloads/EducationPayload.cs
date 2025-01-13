using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record EducationPayload : CreateOrReplaceEducationPayload
{
  public Guid Id { get; set; }
}
