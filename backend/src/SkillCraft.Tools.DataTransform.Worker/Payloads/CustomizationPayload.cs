using SkillCraft.Tools.Core.Customizations.Models;

namespace SkillCraft.Tools.DataTransform.Worker.Payloads;

internal record CustomizationPayload : CreateOrReplaceCustomizationPayload
{
  public Guid Id { get; set; }
}
