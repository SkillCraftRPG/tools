using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class CasteQuerier : ICasteQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<CasteEntity> _castes;
  private readonly IQueryHelper _queryHelper;

  public CasteQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _castes = context.Castes;
    _queryHelper = queryHelper;
  }

  public async Task<CasteModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CasteEntity? caste = await _castes.AsNoTracking()
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return caste == null ? null : await MapAsync(caste, cancellationToken);
  }
  public async Task<CasteModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    CasteEntity? caste = await _castes.AsNoTracking()
      .Include(x => x.Features)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return caste == null ? null : await MapAsync(caste, cancellationToken);
  }

  public async Task<SearchResults<CasteModel>> SearchAsync(SearchCastesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Castes.Table).SelectAll(Castes.Table)
      .ApplyIdFilter(Castes.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Castes.UniqueSlug, Castes.DisplayName);

    if (payload.Skill.HasValue)
    {
      builder.Where(Castes.Skill, Operators.IsEqualTo(payload.Skill.Value));
    }

    IQueryable<CasteEntity> query = _castes.FromQuery(builder).AsNoTracking()
      .Include(x => x.Features);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<CasteEntity>? ordered = null;
    foreach (CasteSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case CasteSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case CasteSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case CasteSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case CasteSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    CasteEntity[] castes = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<CasteModel> items = await MapAsync(castes, cancellationToken);

    return new SearchResults<CasteModel>(items, total);
  }

  private async Task<CasteModel> MapAsync(CasteEntity caste, CancellationToken cancellationToken)
  {
    return (await MapAsync([caste], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<CasteModel>> MapAsync(IEnumerable<CasteEntity> castes, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = castes.SelectMany(caste => caste.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return castes.Select(mapper.ToCaste).ToArray();
  }
}
