using Lua;
using Lua.Unity;

namespace Celeste.Lua
{
    public static class LuaValueExtensions
    {
        public static LuaTable AsTable(this LuaValue[] values)
        {
            return values?.Length == 1 ? values[0].ReadTable(new LuaTable()) : new LuaTable();
        }
    }
}