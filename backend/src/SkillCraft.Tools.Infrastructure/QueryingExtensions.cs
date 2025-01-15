using Logitar.Data;
using Logitar.Portal.Contracts.Search;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Tools.Core;

namespace SkillCraft.Tools.Infrastructure;

internal static class QueryingExtensions
{
  public static IQueryBuilder ApplyFilter(this IQueryBuilder query, TierFilter filter, ColumnId column)
  {
    int[] values = filter.Values.Where(value => value >= 0 && value <= 3).Distinct().ToArray();
    if (values.Length > 0)
    {
      query.Where(column, GetTierOperator(filter.Operator, values));
    }

    return query;
  }
  private static ConditionalOperator GetTierOperator(string @operator, int[] values) => @operator.Trim().ToLowerInvariant() switch
  {
    "gt" => Operators.IsGreaterThan(values.First()),
    "gte" => Operators.IsGreaterThanOrEqualTo(values.First()),
    "in" => Operators.IsIn(values.Select(value => (object)value).ToArray()),
    "lt" => Operators.IsLessThan(values.First()),
    "lte" => Operators.IsLessThanOrEqualTo(values.First()),
    "ne" => Operators.IsNotEqualTo(values.First()),
    "nin" => Operators.IsNotIn(values.Select(value => (object)value).ToArray()),
    _ => Operators.IsEqualTo(values.First()),
  };

  public static IQueryBuilder ApplyIdFilter(this IQueryBuilder query, SearchPayload payload, ColumnId column)
  {
    return ApplyIdFilter(query, payload.Ids, column);
  }
  public static IQueryBuilder ApplyIdFilter(this IQueryBuilder query, IEnumerable<Guid> ids, ColumnId column)
  {
    string[] uniqueIds = ids.Select(id => id.ToString()).Distinct().ToArray();
    if (uniqueIds.Length > 0)
    {
      query.Where(column, Operators.IsIn(uniqueIds));
    }

    return query;
  }

  public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, SearchPayload payload)
  {
    return query.ApplyPaging(payload.Skip, payload.Limit);
  }
  public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int skip, int limit)
  {
    if (skip > 0)
    {
      query = query.Skip(skip);
    }
    if (limit > 0)
    {
      query = query.Take(limit);
    }

    return query;
  }

  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQueryBuilder query) where T : class
  {
    return entities.FromQuery(query.Build());
  }
  public static IQueryable<T> FromQuery<T>(this DbSet<T> entities, IQuery query) where T : class
  {
    return entities.FromSqlRaw(query.Text, query.Parameters.ToArray());
  }
}
