namespace BloodShadow.Core.ModSystem
{
    public struct ModManagerProperty<T>(T instance) : IReadOnlyModManagerProperty<T>
    {
        public readonly T Value => OverrideIndex == -1 ? DefaultValue : OverridesList[_overrideIndex];
        public readonly T DefaultValue = instance;
        public readonly IEnumerable<T> Overrides => OverridesList;
        public int OverrideIndex
        {
            readonly get => _overrideIndex;
            set => _overrideIndex = Math.Clamp(value, -1, OverridesList.Count - 1);
        }

        public readonly List<T> OverridesList;
        private int _overrideIndex = -1;

        public static implicit operator T(ModManagerProperty<T> property) => property.Value;
        public static implicit operator ModManagerProperty<T>(T value) => new(value);
    }
}
