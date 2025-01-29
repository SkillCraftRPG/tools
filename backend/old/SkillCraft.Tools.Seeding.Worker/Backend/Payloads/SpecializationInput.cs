using SkillCraft.Tools.Core.Specializations.Models;

namespace SkillCraft.Tools.Seeding.Worker.Backend.Payloads;

public record SpecializationInput
{
  public Guid Id { get; set; }

  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? RequiredTalent { get; set; }
  public List<string> OtherRequirements { get; set; } = [];

  public List<string> OptionalTalents { get; set; } = [];
  public List<string> OtherOptions { get; set; } = [];

  public ReservedTalentModel? ReservedTalent { get; set; }
}
