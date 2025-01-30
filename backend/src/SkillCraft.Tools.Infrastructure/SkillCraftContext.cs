using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Infrastructure.Entities;

namespace SkillCraft.Tools.Infrastructure;

public class SkillCraftContext : DbContext
{
  public SkillCraftContext(DbContextOptions<SkillCraftContext> options) : base(options)
  {
  }

  internal DbSet<AspectEntity> Aspects => Set<AspectEntity>();
  internal DbSet<CasteEntity> Castes => Set<CasteEntity>();
  internal DbSet<CasteFeatureEntity> CasteFeatures => Set<CasteFeatureEntity>();
  internal DbSet<CustomizationEntity> Customizations => Set<CustomizationEntity>();
  internal DbSet<EducationEntity> Educations => Set<EducationEntity>();
  internal DbSet<FeatureEntity> Features => Set<FeatureEntity>();
  internal DbSet<LanguageEntity> Languages => Set<LanguageEntity>();
  internal DbSet<LanguageScriptEntity> LanguageScripts => Set<LanguageScriptEntity>();
  internal DbSet<LineageEntity> Lineages => Set<LineageEntity>();
  internal DbSet<LineageLanguageEntity> LineageLanguages => Set<LineageLanguageEntity>();
  internal DbSet<LineageTraitEntity> LineageTraits => Set<LineageTraitEntity>();
  internal DbSet<NatureEntity> Natures => Set<NatureEntity>();
  internal DbSet<ScriptEntity> Scripts => Set<ScriptEntity>();
  internal DbSet<SpecializationEntity> Specializations => Set<SpecializationEntity>();
  internal DbSet<SpecializationOptionalTalentEntity> SpecializationOptionalTalents => Set<SpecializationOptionalTalentEntity>();
  internal DbSet<TalentEntity> Talents => Set<TalentEntity>();
  internal DbSet<TraitEntity> Traits => Set<TraitEntity>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
