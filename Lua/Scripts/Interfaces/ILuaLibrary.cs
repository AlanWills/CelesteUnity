#if USE_LUA
using System.Collections.Generic;
using Lua.Standard;
#endif

namespace Celeste.Lua
{
    public interface ILuaLibrary
    {
#if USE_LUA
        string Name { get; }
        IReadOnlyList<LibraryFunction> Functions { get; }
        LuaRuntime LuaRuntime { set; }
#endif
    }
}