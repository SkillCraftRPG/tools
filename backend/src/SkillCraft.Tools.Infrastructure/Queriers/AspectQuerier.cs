using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core.Aspects;
using SkillCraft.Tools.Core.Aspects.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class AspectQuerier : IAspectQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<AspectEntity> _aspects;
  private readonly IQueryHelper _queryHelper;

  public AspectQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _aspects = context.Aspects;
    _queryHelper = queryHelper;
  }

  public async Task<AspectModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    AspectEntity? aspect = await _aspects.AsNoTracking()
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return aspect == null ? null : await MapAsync(aspect, cancellationToken);
  }
  public async Task<AspectModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    AspectEntity? aspect = await _aspects.AsNoTracking()
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return aspect == null ? null : await MapAsync(aspect, cancellationToken);
  }

  public async Task<SearchResults<AspectModel>> SearchAsync(SearchAspectsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Aspects.Table).SelectAll(Aspects.Table)
      .ApplyIdFilter(Aspects.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Aspects.UniqueSlug, Aspects.DisplayName);

    if (payload.Attribute.HasValue)
    {
      ColumnId[] columns = [Aspects.MandatoryAttribute1, Aspects.MandatoryAttribute2, Aspects.OptionalAttribute1, Aspects.OptionalAttribute2];
      builder.WhereOr(columns.Select(column => new OperatorCondition(column, Operators.IsEqualTo(payload.Attribute.Value.ToString()))).ToArray());
    }
    if (payload.Skill.HasValue)
    {
      ColumnId[] columns = [Aspects.DiscountedSkill1, Aspects.DiscountedSkill2];
      builder.WhereOr(columns.Select(column => new OperatorCondition(column, Operators.IsEqualTo(payload.Skill.Value.ToString()))).ToArray());
    }

    IQueryable<AspectEntity> query = _aspects.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<AspectEntity>? ordered = null;
    foreach (AspectSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case AspectSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case AspectSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case AspectSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case AspectSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    AspectEntity[] aspects = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<AspectModel> items = await MapAsync(aspects, cancellationToken);

    return new SearchResults<AspectModel>(items, total);
  }

  private async Task<AspectModel> MapAsync(AspectEntity aspect, CancellationToken cancellationToken)
  {
    return (await MapAsync([aspect], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<AspectModel>> MapAsync(IEnumerable<AspectEntity> aspects, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = aspects.SelectMany(aspect => aspect.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return aspects.Select(mapper.ToAspect).ToArray();
  }
}
