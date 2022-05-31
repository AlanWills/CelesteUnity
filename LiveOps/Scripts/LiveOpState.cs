using System;

namespace Celeste.LiveOps
{
    [Serializable]
    public enum LiveOpState
    {
        Unknown,
        ComingSoon,
        Running,
        Completed,
        Finished
    }
}
