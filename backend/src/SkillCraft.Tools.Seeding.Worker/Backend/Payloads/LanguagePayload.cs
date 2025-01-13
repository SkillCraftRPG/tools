using SkillCraft.Tools.Core.Languages.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

internal record LanguagePayload : CreateOrReplaceLanguagePayload
{
  public Guid Id { get; set; }
}
