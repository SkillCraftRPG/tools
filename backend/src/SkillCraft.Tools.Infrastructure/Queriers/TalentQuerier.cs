using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class TalentQuerier : ITalentQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<TalentEntity> _talents;

  public TalentQuerier(IActorService actorService, SkillCraftContext context)
  {
    _actorService = actorService;
    _talents = context.Talents;
  }

  public async Task<TalentModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _talents.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return talent == null ? null : await MapAsync(talent, cancellationToken);
  }
  public async Task<TalentModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    TalentEntity? talent = await _talents.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return talent == null ? null : await MapAsync(talent, cancellationToken);
  }

  private async Task<TalentModel> MapAsync(TalentEntity talent, CancellationToken cancellationToken)
  {
    return (await MapAsync([talent], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<TalentModel>> MapAsync(IEnumerable<TalentEntity> talents, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = talents.SelectMany(talent => talent.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return talents.Select(mapper.ToTalent).ToArray();
  }
}
