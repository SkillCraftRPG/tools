namespace SkillCraft.Tools.Seeding.Worker.Portal.Payloads;

internal record RolePayload
{
  public Guid Id { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string? DisplayName { get; set; }
  public string? Description { get; set; }
}
