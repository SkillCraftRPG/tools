using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class AspectInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;
  [Name("displayName")]
  public string? DisplayName { get; set; }
  [Name("description")]
  public string? Description { get; set; }

  [Name("attributes.mandatory1")]
  public Ability? MandatoryAttribute1 { get; set; }
  [Name("attributes.mandatory2")]
  public Ability? MandatoryAttribute2 { get; set; }
  [Name("attributes.optional1")]
  public Ability? OptionalAttribute1 { get; set; }
  [Name("attributes.optional2")]
  public Ability? OptionalAttribute2 { get; set; }

  [Name("skills.discounted1")]
  public Skill? DiscountedSkill1 { get; set; }
  [Name("skills.discounted2")]
  public Skill? DiscountedSkill2 { get; set; }
}
