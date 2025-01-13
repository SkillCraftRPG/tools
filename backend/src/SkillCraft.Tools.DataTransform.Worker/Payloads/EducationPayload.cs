using SkillCraft.Tools.Core.Educations.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record EducationPayload : CreateOrReplaceEducationPayload
{
  public Guid Id { get; set; }
}
