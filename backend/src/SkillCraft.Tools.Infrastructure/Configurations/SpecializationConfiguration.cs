using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class SpecializationConfiguration : AggregateConfiguration<SpecializationEntity>, IEntityTypeConfiguration<SpecializationEntity>
{
  public override void Configure(EntityTypeBuilder<SpecializationEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Specializations.Table.Table ?? string.Empty, SkillCraftDb.Specializations.Table.Schema);
    builder.HasKey(x => x.SpecializationId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Tier);
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.RequiredTalentId);
    builder.HasIndex(x => x.ReservedTalentName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
    builder.Property(x => x.ReservedTalentName).HasMaxLength(DisplayName.MaximumLength);

    builder.HasOne(x => x.RequiredTalent).WithMany(x => x.RequiringSpecializations)
      .HasPrincipalKey(x => x.TalentId).HasForeignKey(x => x.RequiredTalentId)
      .OnDelete(DeleteBehavior.SetNull);
    builder.HasMany(x => x.OptionalTalents).WithMany(x => x.OptionalSpecializations)
      .UsingEntity<SpecializationOptionalTalentEntity>(builder =>
      {
        builder.ToTable(SkillCraftDb.SpecializationOptionalTalents.Table.Table ?? string.Empty, SkillCraftDb.SpecializationOptionalTalents.Table.Schema);
        builder.HasKey(x => new { x.SpecializationId, x.TalentId });
      });
  }
}

