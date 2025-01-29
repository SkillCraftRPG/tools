namespace SkillCraft.Tools.Settings;

internal record SessionCookieSettings
{
  public SameSiteMode SameSite { get; set; } = SameSiteMode.Strict;
}
