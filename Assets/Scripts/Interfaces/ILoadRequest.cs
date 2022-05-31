using System;
using System.Collections;

namespace Celeste.Assets
{
    public interface ILoadRequest<T> : IEnumerator, IDisposable where T : UnityEngine.Object
    {
        T Asset { get; }
    }
}
