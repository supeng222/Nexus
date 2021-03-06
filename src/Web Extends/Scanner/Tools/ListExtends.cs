﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Aiursoft.Scanner.Tools
{
    public static class ListExtends
    {
        public static IEnumerable<T> AddWith<T>(this IEnumerable<T> input, T toadd)
        {
            var list = input.ToList();
            list.Add(toadd);
            return list;
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> dbSet, bool enabled, Func<T, bool> predicate) where T : class
        {
            if (enabled)
            {
                return dbSet.Where(predicate);
            }
            else
            {
                return dbSet;
            }
        }
    }
}
