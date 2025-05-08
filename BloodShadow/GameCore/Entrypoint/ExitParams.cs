namespace BloodShadow.GameCore.Entrypoint
{
    public class ExitParams
    {
        public EnterParams TargetSceneEnterParams { get; protected set; }
        public string SourceSceneName { get; private set; }

        public ExitParams(string source)
        {
            SourceSceneName = source;
            TargetSceneEnterParams = new EnterParams();
        }
        public T As<T>() where T : ExitParams { return (T)this; }
    }
}
