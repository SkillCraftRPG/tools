using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Customizations.Events;

namespace SkillCraft.Tools.Core.Customizations;

internal class CustomizationManager : ICustomizationManager
{
  private readonly ICustomizationQuerier _customizationQuerier;
  private readonly ICustomizationRepository _customizationRepository;

  public CustomizationManager(ICustomizationQuerier customizationQuerier, ICustomizationRepository customizationRepository)
  {
    _customizationQuerier = customizationQuerier;
    _customizationRepository = customizationRepository;
  }

  public async Task SaveAsync(Customization customization, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in customization.Changes)
    {
      if (change is CustomizationCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is CustomizationUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      CustomizationId? conflictId = await _customizationQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(customization.Id))
      {
        throw new UniqueSlugAlreadyUsedException(customization, conflictId.Value);
      }
    }

    await _customizationRepository.SaveAsync(customization, cancellationToken);
  }
}
