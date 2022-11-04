using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.TestUtils
{
    public static class EnumerableExtensions
    {
        public static T Random<T>(this IEnumerable<T> list)
        {
            var arrayedList = list.ToArray();
            var random = new Random();
            var randomIndex = random.Next(arrayedList.Count()-1);
            return arrayedList[randomIndex];
        }
    }
}