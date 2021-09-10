using System;
using System.Collections.Generic;
using System.Linq;

namespace Eumel.Dj.Mobile.Data
{
    public static class Randomized
    {
        private static readonly Random randomNumbers = new Random(DateTime.UtcNow.Millisecond);

        public static T Random<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            var sourceArray = source as T[] ?? source.ToArray();
            if (!sourceArray.Any()) throw new ArgumentOutOfRangeException(nameof(source), "source does not contain elements");

            var index = randomNumbers.Next(0, sourceArray.Length);
            return sourceArray.Skip(index).First();
        }

        public static string Random(this Type sourceStatus)
        {
            var sourceArray = Enum.GetNames(sourceStatus);
            if (!sourceArray.Any()) throw new ArgumentOutOfRangeException(nameof(sourceStatus), "source does not contain elements");

            var index = randomNumbers.Next(0, sourceArray.Length);
            return sourceArray.Skip(index).First();
        }
    }
}