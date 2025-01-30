using Logitar.Data;
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
}
