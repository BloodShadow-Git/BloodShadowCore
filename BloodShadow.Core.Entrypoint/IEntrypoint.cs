using R3;

namespace BloodShadow.Core.Entrypoint { public interface IEntrypoint { Observable<ExitParams> Run(EnterParams enterParams); } }
