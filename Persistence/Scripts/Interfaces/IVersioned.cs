using System;

namespace Celeste.Persistence
{
    public interface IVersioned
    {
        int Version { get; set; }
        DateTimeOffset SaveTime { get; set; }

        bool IsLowerVersionThan(IVersioned version);
    }
}