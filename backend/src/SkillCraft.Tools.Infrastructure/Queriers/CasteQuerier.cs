using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Castes;
using SkillCraft.Tools.Core.Castes.Models;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class CasteQuerier : ICasteQuerier
{
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;
  private readonly DbSet<CasteEntity> _castes;

  public CasteQuerier(IActorService actorService, SkillCraftContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _sqlHelper = sqlHelper;
    _castes = context.Castes;
  }

  public async Task<CasteId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug.Value);

    string? streamId = await _castes.AsNoTracking()
      .Where(x => x.UniqueSlugNormalized == uniqueSlugNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new CasteId(streamId);
  }

  public async Task<CasteModel> ReadAsync(Caste caste, CancellationToken cancellationToken)
  {
    return await ReadAsync(caste.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The caste entity 'StreamId={caste.Id}' could not be found.");
  }
  public async Task<CasteModel?> ReadAsync(CasteId casteId, CancellationToken cancellationToken)
  {
    string streamId = casteId.Value;

    CasteEntity? caste = await _castes.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);

    return caste == null ? null : await MapAsync(caste, cancellationToken);
  }
  public async Task<CasteModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    CasteEntity? caste = await _castes.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return caste == null ? null : await MapAsync(caste, cancellationToken);
  }
  public async Task<CasteModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    CasteEntity? caste = await _castes.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return caste == null ? null : await MapAsync(caste, cancellationToken);
  }

  public async Task<SearchResults<CasteModel>> SearchAsync(SearchCastesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Castes.Table).SelectAll(Castes.Table)
      .ApplyIdFilter(payload, Castes.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Castes.UniqueSlug, Castes.DisplayName);

    if (payload.Skill.HasValue)
    {
      builder.Where(Castes.Skill, Operators.IsEqualTo(payload.Skill.Value));
    }

    IQueryable<CasteEntity> query = _castes.FromQuery(builder).AsNoTracking();

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
