using BloodShadow.Core.Operations;

namespace BloodShadow.CoreGame.LevelSwitcher
{
    public abstract class LevelSwitcher
    {
        public abstract event Action OnLevelStartSwitch;
        public abstract event Action OnLevelSwitched;

        public LevelSwitcher()
        {
            OnLevelStartSwitch += () => Console.WriteLine($"{GetType().Name}: Start switch");
            OnLevelSwitched += () => Console.WriteLine($"{GetType().Name}: Level switched");
        }

        public abstract void Load(string scene);
        public abstract void Load(string scene, LoadLevelMode mode);


        public abstract void AsyncLoad(string scene, out Operation operation);
        public abstract void AsyncLoad(string scene, out Operation operation, bool allowSceneActivation);
        public abstract void AsyncLoad(string scene, out Operation operation, LoadLevelMode mode);
        public abstract void AsyncLoad(string scene, out Operation operation, bool allowSceneActivation, LoadLevelMode mode);


        public abstract void AsyncLoad(string scene, out Operation operation, bool allowSceneActivation, params Operation[] additiveTasks);
        public abstract void AsyncLoad(string scene, out Operation operation, LoadLevelMode mode, params Operation[] additiveTasks);
        public abstract void AsyncLoad(string scene, out Operation operation, params Operation[] additiveTasks);
        public abstract void AsyncLoad(string scene, out Operation operation, bool allowSceneActivation, LoadLevelMode mode, params Operation[] additiveTasks);


        public abstract void ForeachLoad(string[] scenes);


        public abstract void ForeachAsyncLoad(string[] scenes, out Operation operation, bool allowSceneActivation);
        public abstract void ForeachAsyncLoad(string[] scenes, out Operation operation);
    }
}
