namespace BloodShadow.Core.Entrypoint
{
    public class EnterParams
    {
        public T As<T>() where T : EnterParams { return (T)this; }
    }
}
