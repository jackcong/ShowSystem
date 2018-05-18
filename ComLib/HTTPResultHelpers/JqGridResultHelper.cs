using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ComLib.Extension;

namespace ComLib.HTTPResultHelpers
{
    public static class JqGridResultHelper
    {
        public static object ToJqGridObject<T>(this IQueryable<T> col, int page, int limit)
        {
            int skipPages = page - 1;
            var res = col.Skip(skipPages*limit);
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            int count = col.ToList().Count();
            return new
                {
                    total = limit > 0 ? Math.Ceiling((double) count/limit) : 1,
                    page = page,
                    records = count,
                    rows = res.ToArray()
                };
        }

        public static object ToJqGridObject<T>(this IEnumerable<T> col, int page, int limit)
        {
            int skipPages = page - 1;
            var res = col.Skip(skipPages * limit);
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            int count = col.Count();
            return new
            {
                total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                page = page,
                records = count,
                rows = res.ToArray()
            };
        }

        public static object ToJqGridObject<T, TRes>(this IQueryable<T> col, int page, int limit, Func<T, TRes> selector)
        {
            try
            {
                int skipPages = page - 1;
                var res = col.Skip(skipPages * limit);
                if (limit > 0)
                {
                    res = res.Take(limit);
                }
                int count = col.Count();
                return new
                    {
                        total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                        page = page,
                        records = count,
                        rows = res.ToArray().Select(selector)
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToJqGridObject<T, TRes>(this IEnumerable<T> col, int page, int limit, Func<T, TRes> selector)
        {
            try
            {
                int skipPages = page - 1;
                var res = col.Skip(skipPages * limit);
                if (limit > 0)
                {
                    res = res.Take(limit);
                }
                int count = col.Count();
                return new
                {
                    total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                    page = page,
                    records = count,
                    rows = res.ToArray().Select(selector)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IQueryable<T> SkipPages<T>(this IQueryable<T> col, int page, int limit)
        {
            int skipPages = page - 1;
            var res = col.Skip(skipPages*limit);
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            return res;
        }

        public static IEnumerable<T> SkipPages<T>(this IEnumerable<T> col, int page, int limit)
        {
            int skipPages = page - 1;
            var res = col.Skip(skipPages*limit);
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            return res;
        }
    }
}
