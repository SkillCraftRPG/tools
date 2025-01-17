using MarkdownSharp;
using SkillCraft.Tools.Core.Languages.Models;
using SkillCraft.Tools.Core.Lineages;
using SkillCraft.Tools.Core.Lineages.Models;

namespace SkillCraft.Tools.Models.Lineage;

internal static class LineageHelper
{
  public static string? FormatAttributes(IAttributeBonuses attributes)
  {
    List<string> formatted = new(capacity: 8);
    if (attributes.Agility > 0)
    {
      formatted.Add($"+{attributes.Agility} en Agilité");
    }
    if (attributes.Coordination > 0)
    {
      formatted.Add($"+{attributes.Coordination} en Coordination");
    }
    if (attributes.Spirit > 0)
    {
      formatted.Add($"+{attributes.Spirit} en Esprit");
    }
    if (attributes.Intellect > 0)
    {
      formatted.Add($"+{attributes.Intellect} en Intellect");
    }
    if (attributes.Presence > 0)
    {
      formatted.Add($"+{attributes.Presence} en Présence");
    }
    if (attributes.Sensitivity > 0)
    {
      formatted.Add($"+{attributes.Sensitivity} en Sensibilité");
    }
    if (attributes.Vigor > 0)
    {
      formatted.Add($"+{attributes.Vigor} en Vigueur");
    }
    if (attributes.Extra > 0)
    {
      formatted.Add($"+{attributes.Extra} au choix");
    }
    return formatted.Count < 1 ? null : string.Join(", ", formatted);
  }

  public static string? FormatLanguages(LanguagesModel languages)
  {
    if (languages.Text != null)
    {
      Markdown markdown = new();
      return markdown.Transform(languages.Text);
    }

    List<string> formatted = new(capacity: 1 + languages.Items.Count);
    foreach (LanguageModel language in languages.Items)
    {
      formatted.Add($"<a href=\"/langues/{language.UniqueSlug}\">{language.DisplayName ?? language.UniqueSlug}</a>");
    }

    if (languages.Extra > 0)
    {
      formatted.Add($"{languages.Extra} au choix");
    }

    return formatted.Count < 1 ? null : string.Join(", ", formatted);
  }

  public static bool HasNames(NamesModel names) => names.Family.Count > 0 || names.Female.Count > 0 || names.Male.Count > 0 || names.Unisex.Count > 0 || names.Custom.Count > 0;
  public static bool HasSpeeds(ISpeeds speeds) => speeds.Walk > 0 || speeds.Climb > 0 || speeds.Swim > 0 || speeds.Fly > 0 || speeds.Hover > 0 || speeds.Hover > 0;
  public static bool HasWeight(WeightModel weight) => weight.Starved != null || weight.Skinny != null || weight.Normal != null || weight.Overweight != null || weight.Obese != null;
  public static bool HasAges(IAges ages) => ages.Adolescent.HasValue && ages.Adult.HasValue && ages.Mature.HasValue && ages.Venerable.HasValue;
}
