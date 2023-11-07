using System.Linq.Expressions;

namespace EcoStore.BLL.Infrastructure;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> Combine<T>(Expression<Func<T, bool>>? first, Expression<Func<T, bool>> second)
    {
        if (first is null)
        {
            return second;
        }

        var body = Expression.AndAlso(first.Body, second.Body);
        return Expression.Lambda<Func<T, bool>>(body, first.Parameters);
    }
}