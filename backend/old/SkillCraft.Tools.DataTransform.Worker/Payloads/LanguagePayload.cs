using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record LanguagePayload : CreateOrReplaceLanguagePayload
{
  public Guid Id { get; set; }
}
