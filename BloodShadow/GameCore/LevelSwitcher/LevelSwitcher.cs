using BloodShadow.Core.Operations;
using System;

namespace BloodShadow.GameCore.LevelSwitcher
{
    public abstract class LevelSwitcher
    {
        public abstract event Action OnLevelStartSwitch;
        public abstract event Action OnLevelSwitched;

        protected const LoadLevelMode DEFAULTLOADMODE = LoadLevelMode.Single;
        protected const bool DEFAULTALLOWLEVELACTIVATION = true;

        public LevelSwitcher()
        {
            OnLevelStartSwitch += () => Console.WriteLine($"{GetType().Name}: Start switch");
            OnLevelSwitched += () => Console.WriteLine($"{GetType().Name}: Level switched");
        }

        public void Load(string level)
            => Load(level, DEFAULTLOADMODE);
        public void AsyncLoad(string level, out Operation operation)
            => AsyncLoad(level, out operation, DEFAULTALLOWLEVELACTIVATION, DEFAULTLOADMODE, Array.Empty<Operation>());
        public void AsyncLoad(string level, out Operation operation, bool allowLevelActivation)
            => AsyncLoad(level, out operation, allowLevelActivation, DEFAULTLOADMODE, Array.Empty<Operation>());
        public void AsyncLoad(string level, out Operation operation, LoadLevelMode mode)
            => AsyncLoad(level, out operation, DEFAULTALLOWLEVELACTIVATION, mode, Array.Empty<Operation>());
        public void AsyncLoad(string level, out Operation operation, bool allowLevelActivation, LoadLevelMode mode)
            => AsyncLoad(level, out operation, allowLevelActivation, mode, Array.Empty<Operation>());
        public void AsyncLoad(string level, out Operation operation, bool allowLevelActivation, params Operation[] additiveTasks)
            => AsyncLoad(level, out operation, allowLevelActivation, DEFAULTLOADMODE, additiveTasks);
        public void AsyncLoad(string level, out Operation operation, LoadLevelMode mode, params Operation[] additiveTasks)
            => AsyncLoad(level, out operation, DEFAULTALLOWLEVELACTIVATION, mode, additiveTasks);
        public void AsyncLoad(string level, out Operation operation, params Operation[] additiveTasks)
            => AsyncLoad(level, out operation, DEFAULTALLOWLEVELACTIVATION, DEFAULTLOADMODE, additiveTasks);
        public void ForeachAsyncLoad(string[] levels, out Operation operation)
            => ForeachAsyncLoad(levels, out operation, DEFAULTALLOWLEVELACTIVATION, Array.Empty<Operation>());
        public void ForeachAsyncLoad(string[] levels, out Operation operation, params Operation[] additiveTasks)
            => ForeachAsyncLoad(levels, out operation, DEFAULTALLOWLEVELACTIVATION, additiveTasks);
        public void ForeachAsyncLoad(string[] levels, out Operation operation, bool allowLevelActivation)
            => ForeachAsyncLoad(levels, out operation, allowLevelActivation, Array.Empty<Operation>());

        public abstract void Load(string level, LoadLevelMode mode);
        public abstract void AsyncLoad(string level, out Operation operation, bool allowLevelActivation, LoadLevelMode mode, params Operation[] additiveTasks);
        public abstract void ForeachLoad(string[] levels);
        public abstract void ForeachAsyncLoad(string[] levels, out Operation operation, bool allowLevelActivation, params Operation[] additiveTasks);
    }
}
