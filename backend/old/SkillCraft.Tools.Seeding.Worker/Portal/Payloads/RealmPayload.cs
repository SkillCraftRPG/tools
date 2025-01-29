namespace SkillCraft.Tools.Seeding.Worker.Portal.Payloads;

internal record RealmPayload
{
  public Guid Id { get; set; }

  public string UniqueSlug { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }

  public string? DefaultLocale { get; set; }
  public string? Url { get; set; }
}
