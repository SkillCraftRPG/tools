namespace SkillCraft.Tools.Settings;

internal record AccessTokenSettings
{
  public int Lifetime { get; set; } = 300;
  public string TokenType { get; set; } = "at+jwt";
}
