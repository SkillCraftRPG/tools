using Logitar.Data;
using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;
using SkillCraft.Tools.Core.Actors;
using SkillCraft.Tools.Core.Lineages;
using SkillCraft.Tools.Core.Lineages.Models;
using SkillCraft.Tools.Infrastructure.Entities;
using SkillCraft.Tools.Infrastructure.SkillCraftDb;

namespace SkillCraft.Tools.Infrastructure.Queriers;

internal class LineageQuerier : ILineageQuerier
{
  private readonly IActorService _actorService;
  private readonly DbSet<LineageEntity> _lineages;
  private readonly ISqlHelper _sqlHelper;

  public LineageQuerier(IActorService actorService, SkillCraftContext context, ISqlHelper sqlHelper)
  {
    _actorService = actorService;
    _lineages = context.Lineages;
    _sqlHelper = sqlHelper;
  }

  public async Task<LineageId?> FindIdAsync(Slug uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug.Value);

    string? streamId = await _lineages.AsNoTracking()
      .Where(x => x.UniqueSlugNormalized == uniqueSlugNormalized)
      .Select(x => x.StreamId)
      .SingleOrDefaultAsync(cancellationToken);

    return streamId == null ? null : new LineageId(streamId);
  }

  public async Task<LineageModel> ReadAsync(Lineage lineage, CancellationToken cancellationToken)
  {
    return await ReadAsync(lineage.Id, cancellationToken)
      ?? throw new InvalidOperationException($"The lineage entity 'StreamId={lineage.Id}' could not be found.");
  }
  public async Task<LineageModel?> ReadAsync(LineageId lineageId, CancellationToken cancellationToken)
  {
    string streamId = lineageId.Value;

    LineageEntity? lineage = await _lineages.AsNoTracking()
      .Include(x => x.Languages)
      .Include(x => x.Parent)
      .SingleOrDefaultAsync(x => x.StreamId == streamId, cancellationToken);

    return lineage == null ? null : await MapAsync(lineage, cancellationToken);
  }
  public async Task<LineageModel?> ReadAsync(Guid id, CancellationToken cancellationToken)
  {
    LineageEntity? lineage = await _lineages.AsNoTracking()
      .Include(x => x.Languages)
      .Include(x => x.Parent)
      .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    return lineage == null ? null : await MapAsync(lineage, cancellationToken);
  }
  public async Task<LineageModel?> ReadAsync(string uniqueSlug, CancellationToken cancellationToken)
  {
    string uniqueSlugNormalized = Helper.Normalize(uniqueSlug);

    LineageEntity? lineage = await _lineages.AsNoTracking()
      .Include(x => x.Languages)
      .Include(x => x.Parent)
      .SingleOrDefaultAsync(x => x.UniqueSlugNormalized == uniqueSlugNormalized, cancellationToken);

    return lineage == null ? null : await MapAsync(lineage, cancellationToken);
  }

  public async Task<SearchResults<LineageModel>> SearchAsync(SearchLineagesPayload payload, CancellationToken cancellationToken)
  {
    IQueryBuilder builder = _sqlHelper.QueryFrom(Lineages.Table).SelectAll(Lineages.Table)
      .ApplyIdFilter(payload, Lineages.Id);
    _sqlHelper.ApplyTextSearch(builder, payload.Search, Lineages.UniqueSlug, Lineages.DisplayName);

    if (payload.ParentId.HasValue)
    {
      TableId parent = new(Lineages.Table.Schema, Lineages.Table.Table ?? string.Empty, "Parent");
      ColumnId parentId = new(nameof(LineageEntity.LineageId), parent);
      ColumnId parentUid = new(nameof(LineageEntity.Id), parent);

      builder.Join(parentId, Lineages.ParentId, new OperatorCondition(parentUid, Operators.IsEqualTo(payload.ParentId.Value)));
    }
    else
    {
      builder.Where(Lineages.ParentId, Operators.IsNull());
    }

    if (payload.Attribute.HasValue)
    {
      ColumnId? column = GetAttributeColumn(payload.Attribute.Value);
      if (column != null)
      {
        builder.Where(column, Operators.IsGreaterThan(0));
      }
    }
    if (payload.LanguageId.HasValue)
    {
      builder.Join(LineageLanguages.LineageId, Lineages.LineageId)
        .Join(SkillCraftDb.Languages.LanguageId, LineageLanguages.LanguageId)
        .Where(SkillCraftDb.Languages.Id, Operators.IsEqualTo(payload.LanguageId.Value));
    }
    if (payload.SizeCategory.HasValue)
    {
      builder.Where(Lineages.SizeCategory, Operators.IsEqualTo(payload.SizeCategory.Value.ToString()));
    }

    IQueryable<LineageEntity> query = _lineages.FromQuery(builder).AsNoTracking();

    long total = await query.LongCountAsync(cancellationToken);

    IOrderedQueryable<LineageEntity>? ordered = null;
    foreach (LineageSortOption sort in payload.Sort)
    {
      switch (sort.Field)
      {
        case LineageSort.CreatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.CreatedOn) : query.OrderBy(x => x.CreatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.CreatedOn) : ordered.ThenBy(x => x.CreatedOn));
          break;
        case LineageSort.DisplayName:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.DisplayName))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.DisplayName) : ordered.ThenBy(x => x.DisplayName));
          break;
        case LineageSort.UniqueSlug:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UniqueSlug) : query.OrderBy(x => x.UniqueSlug))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UniqueSlug) : ordered.ThenBy(x => x.UniqueSlug));
          break;
        case LineageSort.UpdatedOn:
          ordered = (ordered == null)
            ? (sort.IsDescending ? query.OrderByDescending(x => x.UpdatedOn) : query.OrderBy(x => x.UpdatedOn))
            : (sort.IsDescending ? ordered.ThenByDescending(x => x.UpdatedOn) : ordered.ThenBy(x => x.UpdatedOn));
          break;
      }
    }
    query = ordered ?? query;
    query = query.ApplyPaging(payload);

    LineageEntity[] lineages = await query.ToArrayAsync(cancellationToken);
    IReadOnlyCollection<LineageModel> items = await MapAsync(lineages, cancellationToken);

    return new SearchResults<LineageModel>(items, total);
  }
  private static ColumnId? GetAttributeColumn(Ability attribute) => attribute switch
  {
    Ability.Agility => Lineages.Agility,
    Ability.Coordination => Lineages.Coordination,
    Ability.Intellect => Lineages.Intellect,
    Ability.Presence => Lineages.Presence,
    Ability.Sensitivity => Lineages.Sensitivity,
    Ability.Spirit => Lineages.Spirit,
    Ability.Vigor => Lineages.Vigor,
    _ => null,
  };

  private async Task<LineageModel> MapAsync(LineageEntity lineage, CancellationToken cancellationToken)
  {
    return (await MapAsync([lineage], cancellationToken)).Single();
  }
  private async Task<IReadOnlyCollection<LineageModel>> MapAsync(IEnumerable<LineageEntity> lineages, CancellationToken cancellationToken)
  {
    IEnumerable<ActorId> actorIds = lineages.SelectMany(lineage => lineage.GetActorIds());
    IReadOnlyCollection<ActorModel> actors = await _actorService.FindAsync(actorIds, cancellationToken);
    Mapper mapper = new(actors);

    return lineages.Select(mapper.ToLineage).ToArray();
  }
}
