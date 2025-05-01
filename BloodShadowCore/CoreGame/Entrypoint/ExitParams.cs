namespace BloodShadow.CoreGame.Entrypoint
{
    public class ExitParams(string source)
    {
        public EnterParams TargetSceneEnterParams { get; protected set; } = new();
        public string SourceSceneName { get; private set; } = source;

        public T As<T>() where T : ExitParams { return (T)this; }
    }
}
