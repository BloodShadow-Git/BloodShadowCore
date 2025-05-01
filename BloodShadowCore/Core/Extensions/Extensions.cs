namespace BloodShadow.Core.Extensions
{
    public static class Extensions
    {
        public static void Add<T>(this Queue<T> queue, T item) { queue.Enqueue(item); }
    }
}
