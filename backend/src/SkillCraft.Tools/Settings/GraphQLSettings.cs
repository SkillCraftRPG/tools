namespace SkillCraft.Tools.Settings;

internal record GraphQLSettings
{
  public const string SectionKey = "GraphQL";

  public bool EnableMetrics { get; set; }
  public bool ExposeExceptionDetails { get; set; }
}
