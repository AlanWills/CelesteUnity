using Celeste.Persistence;
using System;

namespace Celeste.Log
{
    [Serializable]
    public class HudLogDTO : VersionedDTO
    {
        public int currentHudLogLevel;
    }
}