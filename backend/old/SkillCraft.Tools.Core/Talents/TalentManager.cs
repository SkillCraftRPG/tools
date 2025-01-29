using Logitar.EventSourcing;
using SkillCraft.Tools.Core.Talents.Events;

namespace SkillCraft.Tools.Core.Talents;

internal class TalentManager : ITalentManager
{
  private readonly ITalentQuerier _talentQuerier;
  private readonly ITalentRepository _talentRepository;

  public TalentManager(ITalentQuerier talentQuerier, ITalentRepository talentRepository)
  {
    _talentQuerier = talentQuerier;
    _talentRepository = talentRepository;
  }

  public async Task SaveAsync(Talent talent, CancellationToken cancellationToken)
  {
    Slug? uniqueSlug = null;
    foreach (IEvent change in talent.Changes)
    {
      if (change is TalentCreated created)
      {
        uniqueSlug = created.UniqueSlug;
      }
      else if (change is TalentUpdated updated && updated.UniqueSlug != null)
      {
        uniqueSlug = updated.UniqueSlug;
      }
    }

    if (uniqueSlug != null)
    {
      TalentId? conflictId = await _talentQuerier.FindIdAsync(uniqueSlug, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(talent.Id))
      {
        throw new UniqueSlugAlreadyUsedException(talent, conflictId.Value);
      }
    }

    await _talentRepository.SaveAsync(talent, cancellationToken);
  }
}
