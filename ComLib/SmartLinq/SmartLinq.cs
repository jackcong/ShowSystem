using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ComLib.Extension;

namespace ComLib.SmartLinq
{
    public static partial class SmartLinq
    {
        /// <summary>
        /// Apply SmartLinq query options to the LINQ query.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <param name="param">The query source.</param>
        /// <param name="options">The SmartLinq query options.</param>
        /// <returns>An IQueryable&lt;TSource&gt; that contains the elements satisfying the SmartLinq options.</returns>
        public static IQueryable<TSource> OnQueryOptions<TSource>(this IQueryable<TSource> param, SmartLinqQueryOptions<TSource> options) where TSource : class
        {
            if (options == null)
            {
                return param;
            }
            IQueryable<TSource> res = param;
            if (options.WhereConditions.Length != 0)
            {
                var whereConditions = new List<Expression<Func<TSource, bool>>>();
                options.WhereConditions.Each(
                    c => { if (c != null) whereConditions.Add(c.Expression); });
                res = SmartWhere.OnCondition(param, whereConditions.ToArray());
            }
            if (options.OrderOptions.Length > 0)
            {
                Expression exp = res.Expression;
                string orderAsc = "OrderBy";
                string orderDesc = "OrderByDescending";
                foreach (var c in options.OrderOptions)
                {
                    string orderMethodName;
                    switch (c.OrderType.ToLower())
                    {
                        case "asc":
                        case "ascending":
                            orderMethodName = orderAsc;
                            break;
                        case "desc":
                        case "descending":
                            orderMethodName = orderDesc;
                            break;
                        default:
                            throw new System.Exception("The order type is not supported.");
                    }
                    LambdaExpression le = c.BuildExpression();
                    exp = Expression.Call(typeof (Queryable), orderMethodName,
                                          new[] {typeof (TSource), le.Body.Type},
                                          exp, Expression.Quote(le));
                    orderAsc = "ThenBy";
                    orderDesc = "ThenByDescending";
                    res = param.Provider.CreateQuery<TSource>(exp);
                }
            }
            return res;
        }
        /// <summary>
        /// Create in extension for lambda.
        /// </summary>
        /// <author>
        /// Peter Sun
        /// </author>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertySelector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<TElement> WhereIn<TElement, TValue>(this IQueryable<TElement> source, Expression<Func<TElement, TValue>> propertySelector, params TValue[] values)
        {
            return source.Where(SmartWhere.BuildWhereInExpression(propertySelector, values));
        }    
    }
}
