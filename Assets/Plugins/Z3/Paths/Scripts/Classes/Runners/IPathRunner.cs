using System;

namespace Z3.Paths
{
    public interface IPathRunner : IDisposable
    {
        event Action OnFinish;

        void Start();
        void Update();
    }
}