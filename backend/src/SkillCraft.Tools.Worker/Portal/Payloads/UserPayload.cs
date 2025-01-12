namespace SkillCraft.Tools.Worker.Portal.Payloads;

internal record UserPayload
{
  public Guid Id { get; set; }

  public string UniqueName { get; set; } = string.Empty;
  public string? Password { get; set; }

  public string? FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }

  public string? EmailAddress { get; set; }
  public string? PictureUrl { get; set; }

  public List<string> Roles { get; set; } = [];
}
