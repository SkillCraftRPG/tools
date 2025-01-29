namespace SkillCraft.Tools.Core;

public abstract record Activity : IActivity
{
  public IActivity Anonymize() => this;
}
