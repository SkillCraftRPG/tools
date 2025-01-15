using Logitar.Data;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Specializations
{
  public static readonly TableId Table = new(nameof(SkillCraftContext.Specializations));

  public static readonly ColumnId CreatedBy = new(nameof(SpecializationEntity.CreatedBy), Table);
  public static readonly ColumnId CreatedOn = new(nameof(SpecializationEntity.CreatedOn), Table);
  public static readonly ColumnId StreamId = new(nameof(SpecializationEntity.StreamId), Table);
  public static readonly ColumnId UpdatedBy = new(nameof(SpecializationEntity.UpdatedBy), Table);
  public static readonly ColumnId UpdatedOn = new(nameof(SpecializationEntity.UpdatedOn), Table);
  public static readonly ColumnId Version = new(nameof(SpecializationEntity.Version), Table);

  public static readonly ColumnId Description = new(nameof(SpecializationEntity.Description), Table);
  public static readonly ColumnId DisplayName = new(nameof(SpecializationEntity.DisplayName), Table);
  public static readonly ColumnId Id = new(nameof(SpecializationEntity.Id), Table);
  public static readonly ColumnId SpecializationId = new(nameof(SpecializationEntity.SpecializationId), Table);
  public static readonly ColumnId Tier = new(nameof(SpecializationEntity.Tier), Table);
  public static readonly ColumnId UniqueSlug = new(nameof(SpecializationEntity.UniqueSlug), Table);
  public static readonly ColumnId UniqueSlugNormalized = new(nameof(SpecializationEntity.UniqueSlugNormalized), Table);
  // TODO(fpion): RequiredTalentId
  // TODO(fpion): OtherRequirements
  // TODO(fpion): OptionalTalentIds
  // TODO(fpion): OtherOptions
  // TODO(fpion): ReservedTalent
}
