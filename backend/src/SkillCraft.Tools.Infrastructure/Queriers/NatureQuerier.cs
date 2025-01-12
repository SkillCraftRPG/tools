using Logitar.Data;
using Logitar.EventSourcing;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Actors.Models;
using SkillCraft.Tools.Core.Natures;
using SkillCraft.Tools.Core.Natures.Models;
using SkillCraft.Tools.Core.Search;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class NatureQuerier : INatureQuerier
{
  private readonly IActorService _actorService;
  private readonly ISqlHelper _sqlHelper;
  private readonly DbSet<NatureEntity> _natures;

  public NatureQuerier(IActorService actorService, SkillCraftContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _sqlHelper = sqlHelper;
    _natures = context.Natures;
  }

  public async Task<NatureId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug.Value);

    string? streamId = await _natures.AsNoTracking()
      .Where(x => x.UniqueSlugNormalized == uniqueSlugNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new NatureId(streamId);
  }

  public async Task<NatureModel> ReadAsync(Nature nature, CancellationToken cancellationToken)
  {
    return await ReadAsync(nature.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The nature entity 'StreamId={nature.Id}' could not be found.");
  }
  public async Task<NatureModel?> ReadAsync(NatureId natureId, CancellationToken cancellationToken)
  {
    string streamId = natureId.Value;

    NatureEntity? nature = await _natures.AsNoTracking()
      .Include(x => x.Gift)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);

    return nature == null ? null : await MapAsync(nature, cancellationToken);
  }
  public async Task<NatureModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    NatureEntity? nature = await _natures.AsNoTracking()
      .Include(x => x.Gift)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return nature == null ? null : await MapAsync(nature, cancellationToken);
  }
  public async Task<NatureModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    NatureEntity? nature = await _natures.AsNoTracking()
      .Include(x => x.Gift)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return nature == null ? null : await MapAsync(nature, cancellationToken);
  }

  public async Task<SearchResults<NatureModel>> SearchAsync(SearchNaturesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Natures.Table).SelectAll(Natures.Table)
      .ApplyIdFilter(payload, Natures.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Natures.UniqueSlug, Natures.DisplayName);

    if (payload.Attribute.HasValue)
    {
      builder.Where(Natures.Attribute, Operators.IsEqualTo(payload.Attribute.Value));
    }
    if (payload.GiftId.HasValue)
    {
      builder.Join(
        Customizations.CustomizationId, Natures.GiftId,
        new OperatorCondition(Customizations.Id, Operators.IsEqualTo(payload.GiftId.Value)));
    }

    IQueryable<NatureEntity> query = _natures.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<NatureEntity>? ordered = null;
    foreach (NatureSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case NatureSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case NatureSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case NatureSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case NatureSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    NatureEntity[] natures = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<NatureModel> items = await MapAsync(natures, cancellationToken);

    return new SearchResults<NatureModel>(items, total);
  }

  private async Task<NatureModel> MapAsync(NatureEntity nature, CancellationToken cancellationToken)
  {
    return (await MapAsync([nature], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<NatureModel>> MapAsync(IEnumerable<NatureEntity> natures, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = natures.SelectMany(nature => nature.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return natures.Select(mapper.ToNature).ToArray();
  }
}
