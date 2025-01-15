using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Specializations.Events;

namespace SkillCraft.Tools.Core.Specializations;

internal class SpecializationManager : ISpecializationManager
{
  private readonly ISpecializationQuerier _specializationQuerier;
  private readonly ISpecializationRepository _specializationRepository;

  public SpecializationManager(ISpecializationQuerier specializationQuerier, ISpecializationRepository specializationRepository)
  {
    _specializationQuerier = specializationQuerier;
    _specializationRepository = specializationRepository;
  }

  public async Task SaveAsync(Specialization specialization, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in specialization.Changes)
    {
      if (change is SpecializationCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is SpecializationUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      SpecializationId? conflictId = await _specializationQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(specialization.Id))
      {
        throw new UniqueSlugAlreadyUsedException(specialization, conflictId.Value);
      }
    }

    await _specializationRepository.SaveAsync(specialization, cancellationToken);
  }
}
