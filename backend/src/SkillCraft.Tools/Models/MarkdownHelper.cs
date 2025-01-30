using Logitar;
using MarkdownSharp;

namespace SkillCraft.Tools.Models;

internal static class MarkdownHelper
{
  private static readonly Markdown _markdown = new();

  public static string RenderInline(string value) => _markdown.Transform(value).Remove("<p>").Remove("</p>");
}
