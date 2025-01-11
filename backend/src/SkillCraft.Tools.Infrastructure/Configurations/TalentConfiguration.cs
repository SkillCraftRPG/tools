using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class TalentConfiguration : AggregateConfiguration<TalentEntity>, IEntityTypeConfiguration<TalentEntity>
{
  public override void Configure(EntityTypeBuilder<TalentEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Talents.Table.Table ?? string.Empty, SkillCraftDb.Talents.Table.Schema);
    builder.HasKey(x => x.TalentId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.Tier);
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.AllowMultiplePurchases);
    builder.HasIndex(x => x.RequiredTalentId);
    builder.HasIndex(x => x.Skill);

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Skill).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Skill>());

    builder.HasOne(x => x.RequiredTalent).WithMany(x => x.RequiringTalents)
      .HasPrincipalKey(x => x.TalentId).HasForeignKey(x => x.TalentId)
      .OnDelete(DeleteBehavior.Cascade);
  }
}
