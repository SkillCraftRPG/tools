namespace SkillCraft.Tools.Models.Account;

public record Credentials
{
  public string UniqueName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;

  public Credentials()
  {
  }

  public Credentials(string uniqueName, string password)
  {
    UniqueName = uniqueName;
    Password = password;
  }
}
