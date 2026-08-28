namespace BloodShadow.Core.Entrypoint
{
    public class ExitParams
    {
        public EnterParams TargetSceneEnterParams { get; protected set; }
        public ExitParams() { TargetSceneEnterParams = new EnterParams(); }
        public ExitParams(EnterParams enterParams) : this() { TargetSceneEnterParams = enterParams; }
        public T As<T>() where T : ExitParams { return (T)this; }
    }
}
