namespace BloodShadow.Core
{
    public abstract class Builder<T> : IBuilder<T>
    {
        public abstract bool CloneAvailable { get; }
        public abstract bool BuildAvailable { get; }
        public T Build()
        {
            if (BuildAvailable) { return BuildInternal(); }
            throw new OperationCanceledException("Operation not permited");
        }
        public abstract T BuildInternal();
        public IBuilder<T> Clone()
        {
            if (CloneAvailable) { return CloneInternal(); }
            throw new OperationCanceledException("Operation not permited");
        }
        public abstract IBuilder<T> CloneInternal();
    }
}