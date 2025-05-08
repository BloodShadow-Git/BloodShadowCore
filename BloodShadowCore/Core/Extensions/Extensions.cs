using BloodShadow.Core.ModSystem;

namespace BloodShadow.Core.Extensions
{
    public static class Extensions
    {
        public static void Add<T>(this Queue<T> queue, T item) { queue.Enqueue(item); }
        public static TInput[] FromJaggedArray<TInput>(this TInput[][] input) => [.. input.SelectMany(x => x)];
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][] input) => [.. input.SelectMany(x => x.SelectMany(y => y))];
        public static TInput[] FromJaggedArray<TInput>(this TInput[][][][] input) => [.. input.SelectMany(x => x.SelectMany(y => y.SelectMany(z => z)))];
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);
        public static TOutput[] FromJaggedArray<TInput, TOutput>(this TInput[][][][] input, Converter<TInput, TOutput> converter)
            => Array.ConvertAll(input.FromJaggedArray(), converter);

        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string> converter, bool useNewLine)
        {
            string result = "";
            if (enumerable == null) { return result; }
            int lenght = enumerable.Count();
            for (int i = 0; i < lenght; i++)
            {
                if (i < lenght - 1) { result += $"{converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i).ToString()},{(useNewLine ? '\n' : ' ')}"; }
                else { result += converter?.Invoke(enumerable.ElementAt(i)) ?? enumerable.ElementAt(i).ToString(); }
            }
            return result;
        }
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, Func<T, string> converter) => ArrayToString(enumerable, converter, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable) => ArrayToString(enumerable, null, false);
        public static string ArrayToString<T>(this IEnumerable<T> enumerable, bool useNewLine) => ArrayToString(enumerable, null, useNewLine);

        public static ModManagerProperty<TOutput> Cast<TInput, TOutput>(this ModManagerProperty<TInput> property) where TInput : TOutput => new(property.DefaultValue);

        public static bool Compare(this ModHeader first, ModHeader second, ModVersionDependesType dependesType) => dependesType switch
        {
            ModVersionDependesType.More => first.Version > second.Version,
            ModVersionDependesType.MoreOrEquals => first.Version >= second.Version,
            ModVersionDependesType.Less => first.Version < second.Version,
            ModVersionDependesType.LessOrEquals => first.Version <= second.Version,
            ModVersionDependesType.Equals => first.Version == second.Version,
            _ => false
        };

        public static bool Compare(this ModDependes first, ModHeader second) => first.ModVersionDependesType switch
        {
            ModVersionDependesType.More => first.Header.Version > second.Version,
            ModVersionDependesType.MoreOrEquals => first.Header.Version >= second.Version,
            ModVersionDependesType.Less => first.Header.Version < second.Version,
            ModVersionDependesType.LessOrEquals => first.Header.Version <= second.Version,
            ModVersionDependesType.Equals => first.Header.Version == second.Version,
            _ => false
        };
    }
}
