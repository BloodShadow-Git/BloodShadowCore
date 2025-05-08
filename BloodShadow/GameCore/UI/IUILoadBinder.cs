using BloodShadow.Core.Operations;

namespace BloodShadow.GameCore.UI { public interface IUILoadBinder<TScreen> : IUIBinder<TScreen> { void InternalEnable(Operation op); } }
