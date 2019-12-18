using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class ArrayHelpers
    {
        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
        public static T[] RemoveSequence<T>(this T[] span, in T[] find)
            where T : IEquatable<T>
        {
            var result = new List<T>();

            var scratch = span;
            var length = find.Length;
            int i;
            for (i = 0; i + length < scratch.Length; i++)
            {
                var slice = scratch[i..(i + length)];
                if (slice.SequenceEqual(find))
                {
                    scratch = scratch[(i + length)..];
                    i = -1;
                }
                else
                {
                    result.Add(scratch[i]);
                }
            }

            if (i < scratch.Length)
            {
                result.AddRange(scratch[i..]);
            }

            return result.ToArray();
        }

        public static T[] ReplaceSequence<T>(this T[] span, in T[] find, T replace)
            where T : IEquatable<T>
        {
            var result = new List<T>();

            var scratch = span;
            var length = find.Length;
            int i;
            for (i = 0; i + length <= scratch.Length; i++)
            {
                var slice = scratch[i..(i + length)];
                if (slice.SequenceEqual(find))
                {
                    scratch = scratch[(i + length)..];
                    i = -1;
                    result.Add(replace);
                }
                else
                {
                    result.Add(scratch[i]);
                }
            }

            if (i < scratch.Length)
            {
                result.AddRange(scratch[i..]);
            }

            return result.ToArray();
        }
    }
}
