using System;
using System.Collections.Generic;
using System.Linq;

namespace BloodShadow.Core.Extensions
{
    public static class Extensions
    {
        public static TInput[] FromJaggedArray<TInput>(this TInput[][] input) => input.SelectMany(x => x).ToArray();
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][] input) => input.SelectMany(x => x.SelectMany(y => y)).ToArray();
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][][] input) => input.SelectMany(x => x.SelectMany(y => y.SelectMany(z => z))).ToArray();
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);

        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string>? converter, bool useNewLine)
        {
            string result = "";
            if (enumerable == null) { return result; }
            int lenght = enumerable.Count();
            for (int i = 0; i < lenght; i++)
            {
                if (i < lenght - 1) { result += $"{converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i)?.ToString() ?? ""},{(useNewLine ? '\n' : ' ')}"; }
                else { result += converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i)?.ToString() ?? ""; }
            }
            return result;
        }
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string> converter) => ArrayToString(enumerable, converter, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable) => ArrayToString(enumerable, null, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, bool useNewLine) => ArrayToString(enumerable, null, useNewLine);
    }
}
