using Logitar.Identity.Core;
using Logitar.Identity.EntityFrameworkCore.Relational.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure.Configurations;

internal class ScriptConfiguration : AggregateConfiguration<ScriptEntity>, IEntityTypeConfiguration<ScriptEntity>
{
  public override void Configure(EntityTypeBuilder<ScriptEntity> builder)
  {
    base.Configure(builder);

    builder.ToTable(SkillCraftDb.Scripts.Table.Table ?? string.Empty, SkillCraftDb.Scripts.Table.Schema);
    builder.HasKey(x => x.ScriptId);

    builder.HasIndex(x => x.Id).IsUnique();
    builder.HasIndex(x => x.UniqueSlug);
    builder.HasIndex(x => x.UniqueSlugNormalized).IsUnique();
    builder.HasIndex(x => x.DisplayName);

    builder.Property(x => x.UniqueSlug).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.UniqueSlugNormalized).HasMaxLength(UniqueName.MaximumLength);
    builder.Property(x => x.DisplayName).HasMaxLength(DisplayName.MaximumLength);
  }
}
