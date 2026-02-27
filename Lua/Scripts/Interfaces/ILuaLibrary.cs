using System.Collections.Generic;
using Lua.Standard;

namespace Celeste.Lua
{
    public interface ILuaLibrary
    {
        string Name { get; }
        IReadOnlyList<LibraryFunction> Functions { get; }
        LuaRuntime LuaRuntime { set; }
    }
}