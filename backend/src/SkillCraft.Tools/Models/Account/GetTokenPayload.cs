namespace SkillCraft.Tools.Models.Account;

public record GetTokenPayload
{
  [JsonPropertyName("refresh_token")]
  public string? RefreshToken { get; set; }

  public Credentials? Credentials { get; set; }
}
