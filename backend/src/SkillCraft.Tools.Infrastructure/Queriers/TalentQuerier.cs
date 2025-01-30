using Logitar.Cms.Core.Actors;
using Logitar.Cms.Core.Search;
using Logitar.Cms.Infrastructure;
using Logitar.Cms.Infrastructure.Actors;
using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Talents;
using SkillCraft.Tools.Core.Talents.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class TalentQuerier : ITalentQuerier
{
  private readonly IActorService _actorService;
  private readonly IQueryHelper _queryHelper;
  private readonly DbSet<TalentEntity> _talents;

  public TalentQuerier(IActorService actorService, SkillCraftContext context, IQueryHelper queryHelper)
  {
    _actorService = actorService;
    _queryHelper = queryHelper;
    _talents = context.Talents;
  }

  public async Task<TalentModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    TalentEntity? talent = await _talents.AsNoTracking()
      .Include(x => x.RequiredTalent)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return talent == null ? null : await MapAsync(talent, cancellationToken);
  }
  public async Task<TalentModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    TalentEntity? talent = await _talents.AsNoTracking()
      .Include(x => x.RequiredTalent)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return talent == null ? null : await MapAsync(talent, cancellationToken);
  }

  public async Task<SearchResults<TalentModel>> SearchAsync(SearchTalentsPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _queryHelper.From(Talents.Table).SelectAll(Talents.Table)
      .ApplyIdFilter(Talents.Id, payload.Ids);
    _queryHelper.ApplyTextSearch(builder, payload.Search, Talents.UniqueSlug, Talents.DisplayName);

    if (payload.AllowMultiplePurchases.HasValue)
    {
      builder.Where(Talents.AllowMultiplePurchases, Operators.IsEqualTo(payload.AllowMultiplePurchases.Value));
    }
    if (payload.RequiredTalentId.HasValue)
    {
      TableId required = new(Talents.Table.Schema, Talents.Table.Table ?? string.Empty, "Required");
      builder.Join(
        new ColumnId(Talents.TalentId.Name ?? string.Empty, required),
        Talents.RequiredTalentId,
        new OperatorCondition(
          new ColumnId(Talents.Id.Name ?? string.Empty, required),
          Operators.IsEqualTo(payload.RequiredTalentId.Value)));
    }
    if (!string.IsNullOrWhiteSpace(payload.Skill))
    {
      if (bool.TryParse(payload.Skill, out bool hasSkill))
      {
        builder.Where(Talents.Skill, hasSkill ? Operators.IsNotNull() : Operators.IsNull());
      }
      else if (Enum.TryParse(payload.Skill, ignoreCase: true, out Skill skill))
      {
        builder.Where(Talents.Skill, Operators.IsEqualTo(skill.ToString()));
      }
      else
      {
        builder.Where(Talents.Skill, Operators.IsEqualTo(payload.Skill.Trim()));
      }
    }
    if (payload.Tier != null)
    {
      builder.ApplyFilter(payload.Tier, Talents.Tier);
    }

    IQueryable<TalentEntity> query = _talents.FromQuery(builder).AsNoTracking()
      .Include(x => x.RequiredTalent);

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<TalentEntity>? ordered = null;
    foreach (TalentSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case TalentSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case TalentSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case TalentSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case TalentSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    TalentEntity[] talents = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<TalentModel> items = await MapAsync(talents, cancellationToken);

    return new SearchResults<TalentModel>(items, total);
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
