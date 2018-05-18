using System;
using System.Linq.Expressions;

namespace ComLib.SmartLinq
{
    /// <summary>
    /// Provides SmartLinq query options that can hold one or more lambda predicates and/or ordering options.
    /// </summary>
    /// <typeparam name="TSource">The type of the data source.</typeparam>
    public class SmartLinqQueryOptions<TSource> where TSource : class
    {
        public SmartLinqWhereCondition<TSource>[] WhereConditions { get; set; }
        public SmartLinqOrderOption<TSource>[] OrderOptions { get; set; }

        public SmartLinqQueryOptions(SmartLinqWhereCondition<TSource>[] @where = null,
                                     SmartLinqOrderOption<TSource>[] order = null)
        {
            if (@where != null)
            {
                WhereConditions = new SmartLinqWhereCondition<TSource>[@where.Length];
                @where.CopyTo(WhereConditions, 0);
            }
            if (order != null)
            {
                OrderOptions = new SmartLinqOrderOption<TSource>[order.Length];
                order.CopyTo(OrderOptions, 0);
            }
        }
    }

    /// <summary>
    /// Represents a SmartLinq where condition that contains a lambda predicate.
    /// </summary>
    /// <typeparam name="T">The type of the data source that this condition will be applied to.</typeparam>
    public class SmartLinqWhereCondition<T> where T : class
    {
        private readonly Expression<Func<T,bool>>  _expression;

        public Expression<Func<T,bool>> Expression
        {
            get { return _expression; }
        } 

        /// <summary>
        /// Creates a SmartLinq where condition with a given predicate expression.
        /// </summary>
        /// <param name="expression">The predicate.</param>
        public SmartLinqWhereCondition(Expression<Func<T,bool>>  expression)
        {
            _expression = expression;
        }
    }

    /// <summary>
    /// Represents a SmartLinq ordering option.
    /// </summary>
    /// <typeparam name="T">The type of the data source that this option will be applied to.</typeparam>
    public class SmartLinqOrderOption<T> where T : class
    {
        public string OrderBy { get; set; }
        public string OrderType { get; set; }

        /// <summary>
        /// Creates a SmartLinq ordering option.
        /// </summary>
        /// <param name="orderBy">The property name of T that will be applied in ordering.</param>
        /// <param name="orderType">The ordering direction.</param>
        public SmartLinqOrderOption(string orderBy, string orderType = "asc")
        {
            OrderBy = orderBy;
            OrderType = orderType;
        }

        /// <summary>
        /// Builds an expression that gets the value of the property given from parameter orderBy in T
        /// </summary>
        /// <returns>The built lambda expression.</returns>
        public LambdaExpression BuildExpression()
        {
            ParameterExpression pe = Expression.Parameter(typeof (T), "x");
            Expression exp = pe;
            foreach(var c in OrderBy.Split(new[]{'.'}, StringSplitOptions.RemoveEmptyEntries))
            {
                exp = Expression.PropertyOrField(exp, c.Trim());
            }
            return Expression.Lambda(exp, pe);
        }

    }
}
