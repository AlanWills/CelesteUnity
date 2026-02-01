#if USE_LUA
using System;
using System.Collections.Generic;
using Lua.Unity;

namespace Celeste.Lua
{
    [Serializable]
    public struct LuaScriptAndVariables
    {
        public LuaScript Script;
        public List<ScriptVariable> Variables;
    }
}
#endif