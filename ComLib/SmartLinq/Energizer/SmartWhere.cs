using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace ComLib.SmartLinq
{
    public static partial class SmartLinq
    {
        private static class SmartWhere
        {

            // TODO: Unit test this method.
            public static IQueryable<TSource> OnCondition<TSource>(IQueryable<TSource> param,
                                                                   params Expression<Func<TSource, bool>>[] func)
            {
                ParameterExpression parC = Expression.Parameter(typeof (TSource), "c");
                var acv = new ExpressionVisitors.AggregateConditionsVisitor(parC);
                Expression exp = Expression.Constant(true);
                foreach (var c in func)
                {
                    if (c != null)
                    {
                        var be = c.Update(c.Body, new[] {parC});
                        exp = Expression.AndAlso(exp, acv.Visit(be.Body));
                    }
                }
                Expression<Func<TSource, bool>> final = Expression.Lambda<Func<TSource, bool>>(exp, parC);
                return param.Where(final);
            }
            public static IQueryable<TSource> OnConditionWithOr<TSource>(IQueryable<TSource> param,
                                                                   params Expression<Func<TSource, bool>>[] func)
            {
                ParameterExpression parC = Expression.Parameter(typeof(TSource), "c");
                var acv = new ExpressionVisitors.AggregateConditionsVisitor(parC);
                Expression exp = Expression.Constant(true);
                foreach (var c in func)
                {
                    if (c != null)
                    {
                        var be = c.Update(c.Body, new[] { parC });
                        exp = Expression.Or(exp, acv.Visit(be.Body));
                    }
                }
                Expression<Func<TSource, bool>> final = Expression.Lambda<Func<TSource, bool>>(exp, parC);
                return param.Where(final);
            }
            public static Expression<Func<TElement, bool>> BuildWhereInExpression<TElement, TValue>(Expression<Func<TElement, TValue>> propertySelector, IEnumerable<TValue> values)
            {
                ParameterExpression p = propertySelector.Parameters.Single();
                if (!values.Any())
                    return e => false;

                var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
                var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

                return Expression.Lambda<Func<TElement, bool>>(body, p);
            }
        }
    }
}
