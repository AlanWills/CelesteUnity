using System;

namespace Celeste.Lua
{
    [Serializable]
    public struct ScriptVariable
    {
        public string Name;
        public UnityEngine.Object Value;
    }
}