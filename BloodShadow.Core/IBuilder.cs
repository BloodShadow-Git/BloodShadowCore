namespace BloodShadow.Core
{
    public interface IBuilder<out T>
    {
        bool CloneAvailable { get; }
        bool BuildAvailable { get; }
        public T Build();
        public IBuilder<T> Clone();
    }
}