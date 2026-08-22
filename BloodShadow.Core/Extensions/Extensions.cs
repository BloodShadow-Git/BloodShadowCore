namespace BloodShadow.Core.Extensions
{
    public static partial class Extensions
    {
        #region JaggedArray
        public static TInput[] FromJaggedArray<TInput>(this TInput[][] input) => [.. input.SelectMany(x => x)];
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][] input) => [.. input.SelectMany(x => x.SelectMany(y => y))];
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][][] input) => [.. input.SelectMany(x => x.SelectMany(y => y.SelectMany(z => z)))];
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        #endregion

        #region ArrayToString
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string>? converter, bool useNewLine)
        {
            string result = "";
            if (enumerable == null) { throw new NullReferenceException("Collection was null"); }
            int length = enumerable.Count();
            for (int i = 0; i < length; i++)
            {
                if (i < length - 1) { result += $"{converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i)?.ToString() ?? ""},{(useNewLine ? '\n' : ' ')}"; }
                else { result += converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i)?.ToString() ?? ""; }
            }
            return result;
        }
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string> converter) => ArrayToString(enumerable, converter, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable) => ArrayToString(enumerable, default, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, bool useNewLine) => ArrayToString(enumerable, default, useNewLine);
        #endregion

        #region Checks
        public static bool Valid(this string source) => !string.IsNullOrEmpty(source) && !string.IsNullOrWhiteSpace(source);
        public static bool Valid(this byte[] source) => source.Length > 0 && source.Distinct().Count() > 0;
        #endregion
    }
}
