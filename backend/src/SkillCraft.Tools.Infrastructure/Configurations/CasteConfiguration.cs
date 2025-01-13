using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class CasteConfiguration : AggregateConfiguration<CasteEntity>, IEntityTypeConfiguration<CasteEntity>
{
  public override void Configure(EntityTypeBuilder<CasteEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Castes.Table.Table ?? string.Empty, SkillCraftDb.Castes.Table.Schema);
    builder.HasKey(x => x.CasteId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);
    builder.HasIndex(x => x.Skill);

    builder.Property(x => x.UniqueSlug).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.DisplayName).HasMaxLength(byte.MaxValue);
    builder.Property(x => x.Skill).HasMaxLength(byte.MaxValue).HasConversion(new EnumToStringConverter<Skill>());
    // TODO(fpion): WealthRoll
    // TODO(fpion): Features
  }
}
