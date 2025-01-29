namespace SkillCraft.Tools.Models.Components;

public record BreadcrumbModel
{
  public string? Current { get; set; }
  public List<LinkModel> Links { get; set; } = [];

  public bool IsHome => Current == null && Links.Count < 1;

  public BreadcrumbModel(string? current = null, IEnumerable<LinkModel>? links = null)
  {
    Current = current;

    if (links != null)
    {
      Links.AddRange(links);
    }
  }
}
