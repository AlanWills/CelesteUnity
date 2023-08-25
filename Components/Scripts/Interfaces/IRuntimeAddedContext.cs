using System;

namespace Celeste.Components
{
    public interface IRuntimeAddedContext
    {
        public static IRuntimeAddedContext Empty => new RuntimeAddedEmptyContext();
    }

    [Serializable]
    public struct RuntimeAddedEmptyContext : IRuntimeAddedContext { }

}
