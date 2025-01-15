namespace SkillCraft.Tools.Core.Specializations.Models;

public record CreateOrReplaceSpecializationPayload
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public Guid? RequiredTalentId { get; set; }
  public List<Guid> OptionalTalentIds { get; set; } = [];
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OtherOptions
  public ReservedTalentModel? ReservedTalent { get; set; }
}
