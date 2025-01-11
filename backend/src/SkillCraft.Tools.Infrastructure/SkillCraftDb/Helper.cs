namespace SkillCraft.Tools.Infrastructure.SkillCraftDb;

internal static class Helper
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
