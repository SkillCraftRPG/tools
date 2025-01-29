using CsvHelper.Configuration.Attributes;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.DataTransform.Worker.Inputs;

internal class NatureInput
{
  [Name("id")]
  public Guid Id { get; set; }

  [Name("uniqueSlug")]
  public string UniqueSlug { get; set; } = string.Empty;

  [Name("displayName")]
  public string? DisplayName { get; set; }

  [Name("description")]
  public string? Description { get; set; }

  [Name("attribute")]
  public Ability? Attribute { get; set; }

  [Name("giftId")]
  public Guid? GiftId { get; set; }
}
