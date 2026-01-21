using System;
using System.Collections.Generic;
using Lua.Unity;

namespace Celeste.Lua
{
    [Serializable]
    public readonly struct LuaScriptAndVariables
    {
        public readonly LuaScript Script;
        public readonly List<ScriptVariable> Variables;
    }
}