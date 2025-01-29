using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Educations.Events;

namespace SkillCraft.Tools.Core.Educations;

internal class EducationManager : IEducationManager
{
  private readonly IEducationQuerier _educationQuerier;
  private readonly IEducationRepository _educationRepository;

  public EducationManager(IEducationQuerier educationQuerier, IEducationRepository educationRepository)
  {
    _educationQuerier = educationQuerier;
    _educationRepository = educationRepository;
  }

  public async Task SaveAsync(Education education, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in education.Changes)
    {
      if (change is EducationCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is EducationUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      EducationId? conflictId = await _educationQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(education.Id))
      {
        throw new UniqueSlugAlreadyUsedException(education, conflictId.Value);
      }
    }

    await _educationRepository.SaveAsync(education, cancellationToken);
  }
}
