using Logitar.Portal.Contracts;

namespace SkillCraft.Tools.Core.Specializations.Models;

public class SpecializationModel : AggregateModel
{
  public int Tier { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  // TODO(fpion): RequiredTalent
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalents
  // TODO(fpion): OtherOptions
  public ReservedTalentModel? ReservedTalent { get; set; }

  public override string ToString() => $"{DisplayName ?? UniqueSlug} | {base.ToString()}";
}
