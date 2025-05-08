using R3;
using System;

namespace BloodShadow.GameCore.Entrypoint
{
    public abstract class Entrypoint<T> : IEntrypoint where T : EnterParams
    {
        public Observable<ExitParams> Run(EnterParams enterParams)
        {
            Console.WriteLine($"Running {GetType().Name} entrypoint");
            return Init((T)enterParams);
        }

        protected abstract Observable<ExitParams> Init(T enterParams);
    }
}
