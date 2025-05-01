using R3;

namespace BloodShadow.CoreGame.Entrypoint { public interface IEntrypoint { Observable<ExitParams> Run(EnterParams enterParams); } }
