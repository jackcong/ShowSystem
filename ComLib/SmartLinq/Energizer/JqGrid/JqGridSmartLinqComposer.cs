/**********************
 * Author: Kenneth    *
 * Date:   2011/12/31 *
 * Org:    JCC        *
 **********************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ComLib.Converter;
using ComLib.Extension;

namespace ComLib.SmartLinq.Energizer.JqGrid
{
    /// <summary>
    /// Provides functionality to create SmartLinq query options from JqGrid search/ordering arguments.
    /// </summary>
    public static class JqGridSmartLinqComposer
    {
        /// <summary>
        /// Creates a SmartLinq where condition by from given JqGrid search arguments.
        /// </summary>
        /// <typeparam name="T">The type of the data that this condition applies to.</typeparam>
        /// <param name="field">The JqGrid search field (corresponding to a property of T).</param>
        /// <param name="oper">The JqGrid operator. Please refer to JqGrid source code for a complete list of available operators.</param>
        /// <param name="value">The JqGrid search value.</param>
        /// <returns>The created SmartLinq where condition.</returns>
        public static SmartLinqWhereCondition<T> ComposeWhereCondition<T>(string field, string oper, string value)
            where T : class
        {

            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(oper))
                return null;
            ParameterExpression member = Expression.Parameter(typeof (T), "x");
            Expression prop = Expression.PropertyOrField(member, field);
            try
            {
                Type dataType = ((MemberExpression) prop).Member.MemberType == MemberTypes.Property
                                    ? ((PropertyInfo) ((MemberExpression) prop).Member).PropertyType
                                    : ((FieldInfo) ((MemberExpression) prop).Member).FieldType;
                Type nullableType;
                Type underlyingType;
                if (dataType.IsValueType)
                {
                    if (dataType.IsGenericType && dataType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    {
                        nullableType = dataType;
                        underlyingType = Nullable.GetUnderlyingType(dataType);
                    }
                    else
                    {
                        nullableType = typeof (Nullable<>).MakeGenericType(dataType);
                        underlyingType = dataType;
                        prop = Expression.Convert(prop, nullableType);
                    }

                }
                else
                {
                    nullableType = underlyingType = dataType;
                }

                var values = new List<object>();
                var consts = new List<Expression>();
                Expression expr;

                switch (oper)
                {
                    case "bt":
                        value.Split(new[] {'&'}).Each(values.Add);
                        break;
                    default:
                        values.Add(value);
                        break;
                }

                if (underlyingType == typeof (DateTime))
                {
                    //value = value.ToDateTime();
                    //value = "\""+value+"\"" + ".ToDateTime()";
                    values[0] = string.IsNullOrEmpty(values[0] as string)
                                    ? null
                                    : (DateTime?) (values[0] as string).ToDateTime();
                    if (values.Count > 1)
                        values[1] = string.IsNullOrEmpty(values[1] as string)
                                        ? null
                                        : (DateTime?) (values[1] as string).ToDateTime();
                } else if(underlyingType == typeof(string))
                {
                    
                }
                else if (underlyingType.GetInterface("IComparable") != null)
                {
                    values[0] = string.IsNullOrEmpty(values[0] as string)
                                    ? null
                                    : Convert.ChangeType(values[0], underlyingType);
                    if (values.Count > 1)
                        values[1] = string.IsNullOrEmpty(values[1] as string)
                                        ? null
                                        : Convert.ChangeType(values[1], underlyingType);
                }

                values.Each(c => consts.Add(Expression.Constant(c, nullableType)));

                switch (oper)
                {
                    case "eq":
                        {
                            expr = Expression.Equal(prop, consts[0]);
                            break;
                        }
                    case "ne":
                        {
                            expr = Expression.NotEqual(prop, consts[0]);
                            break;
                        }
                    case "bw":
                        {
                            expr = Expression.Call(prop, "StartsWith", null, consts[0]);
                            break;
                        }
                    case "bn":
                        {
                            expr = Expression.Call(prop, "StartsWith", null, consts[0]);

                            // Don't know why Expression.IsFalse won't work.
                            expr = Expression.Equal(expr, Expression.Constant(false));
                            break;
                        }
                    case "ew":
                        {
                            expr = Expression.Call(prop, "EndsWith", null, consts[0]);
                            break;
                        }
                    case "en":
                        {
                            expr = Expression.Call(prop, "EndsWith", null, consts[0]);

                            // Don't know why Expression.IsFalse won't work.
                            expr = Expression.Equal(expr, Expression.Constant(false));
                            break;
                        }
                    case "cn":
                        {
                            expr = Expression.Call(prop, "Contains", null, consts[0]);
                            break;
                        }
                    case "nc":
                        {
                            expr = Expression.Call(prop, "Contains", null, consts[0]);

                            // Don't know why Expression.IsFalse won't work.
                            expr = Expression.Equal(expr, Expression.Constant(false));
                            break;
                        }
                    case "gt":
                        {
                            expr = Expression.GreaterThan(prop, consts[0]);
                            break;
                        }
                    case "ge":
                        {
                            expr = Expression.GreaterThanOrEqual(prop, consts[0]);
                            break;
                        }
                    case "lt":
                        {
                            expr = Expression.LessThan(prop, consts[0]);
                            break;
                        }
                    case "le":
                        {
                            expr = Expression.LessThanOrEqual(prop, consts[0]);
                            break;
                        }
                    case "nu":
                        {
                            expr = Expression.Equal(prop, Expression.Constant(null));
                            break;
                        }
                    case "nn":
                        {
                            expr = Expression.NotEqual(prop, Expression.Constant(null));
                            break;
                        }
                    case "bt":
                        {
                            Expression expr1 = Expression.GreaterThanOrEqual(prop, consts[0]);
                            Expression expr2;
                            if (underlyingType == typeof (DateTime))
                            {
                                expr2 = Expression.LessThan(prop, Expression.Constant(values[1] == null
                                                                                          ? null
                                                                                          : ((DateTime?)
                                                                                             ((DateTime) values[1]).
                                                                                                 AddDays(
                                                                                                     1)), prop.Type));
                            }
                            else
                            {
                                expr2 = Expression.LessThanOrEqual(prop, consts[1]);
                            }

                            expr = Expression.AndAlso(expr1, expr2);
                            break;
                        }
                    case "ert":
                        {
                            if (underlyingType == typeof (DateTime))
                            {
                                expr = Expression.LessThan(prop, consts[0]);
                                break;
                            }
                            else
                            {
                                throw new System.Exception("This data type does not support operation `earlier than`");
                            }
                        }
                    case "ere":
                        {
                            if (underlyingType == typeof (DateTime))
                            {
                                expr = Expression.LessThan(prop,
                                                           Expression.Constant(
                                                               values[0] == null
                                                                   ? null
                                                                   : ((DateTime?) ((DateTime) values[0]).AddDays(1)),
                                                               prop.Type));
                                break;
                            }
                            else
                            {
                                throw new System.Exception("This data type does not support operation `earlier than`");
                            }
                        }
                    case "ltt":
                        {
                            if (underlyingType == typeof (DateTime))
                            {
                                expr = Expression.GreaterThanOrEqual(prop, Expression.Constant(values[0] == null
                                                                                                   ? null
                                                                                                   : ((DateTime?)
                                                                                                      ((DateTime)
                                                                                                       values[0])
                                                                                                          .AddDays(1)),
                                                                                               prop.Type));
                                break;
                            }
                            else
                            {
                                throw new System.Exception("This data type does not support operation `earlier than`");
                            }
                        }
                    case "lte":
                        {
                            if (underlyingType == typeof (DateTime))
                            {
                                expr = Expression.GreaterThanOrEqual(prop, consts[0]);
                                break;
                            }
                            else
                            {
                                throw new System.Exception("This data type does not support operation `earlier than`");
                            }
                        }
                    default:
                        return null;
                }
                expr = Expression.Lambda<Func<T, bool>>(expr, member);
                return new SmartLinqWhereCondition<T>((Expression<Func<T, bool>>) expr);
            }
            catch (System.Exception e)
            {
                // On error, returns no result.
                return
                    new SmartLinqWhereCondition<T>(Expression.Lambda<Func<T, bool>>(Expression.Constant(false), member));
            }
        }

        /// <summary>
        /// Creates a SmartLinq ordering option from Jqgrid ordering arguments.
        /// </summary>
        /// <typeparam name="T">The type of the data that this option applies to.</typeparam>
        /// <param name="orderBy">The JqGrid field whose corresponding property in T will be applied in ordering.</param>
        /// <param name="orderType">The ordering direction.</param>
        /// <returns>The created SmartLinq ordering option.</returns>
        public static SmartLinqOrderOption<T> ComposeOrderOption<T>(string orderBy, string orderType) where T : class
        {
            return new SmartLinqOrderOption<T>(orderBy, orderType);
        }
    }
}