using System;

namespace Celeste.Lua
{
    [Serializable]
    public readonly struct ScriptVariable
    {
        public readonly string Name;
        public readonly UnityEngine.Object Value;
    }
}