using R3;

namespace BloodShadow.GameCore.Entrypoint { public interface IEntrypoint { Observable<ExitParams> Run(EnterParams enterParams); } }
