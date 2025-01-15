namespace SkillCraft.Tools.Core.Specializations.Models;

public record CreateOrReplaceSpecializationPayload
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): RequiredTalentId
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  // TODO(fpion): ReservedTalent
}
