namespace SkillCraft.Tools.Core.Customizations;

public interface ICustomizationManager
{
  Task SaveAsync(Customization customization, CancellationToken cancellationToken = default);
}
