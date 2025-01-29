namespace SkillCraft.Tools.Infrastructure.MongoDB.Settings;

internal record MongoDBSettings
{
  public const string SectionKey = "MongoDB";

  public string ConnectionString { get; set; } = string.Empty;
  public string DatabaseName { get; set; } = string.Empty;
}
