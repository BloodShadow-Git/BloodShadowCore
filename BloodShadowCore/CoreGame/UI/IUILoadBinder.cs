using BloodShadow.Core.Operations;

namespace BloodShadow.CoreGame.UI { public interface IUILoadBinder<TScreen> : IUIBinder<TScreen> { void InternalEnable(Operation op); } }
