using System;
using System.Collections.Generic;
using System.Linq;

namespace VBSharper.Plugins.Core.ExtensionMethods
{
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        /// Returns true if a sequence is null or does not contain any elements.
        /// </summary>
        /// <param name="source">The IEnumerable&lt;T&gt; to check for emptiness.</param>
        public static bool None<TSource>(this IEnumerable<TSource> source) {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Returns true if a sequence is null or does not contain any elements that meet the predicate condition.
        /// </summary>
        /// <param name="source">An IEnumerable&lt;T&gt; whose elements to apply the predicate to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
        {
            return source == null || !source.Any(predicate);
        }
    }
}
