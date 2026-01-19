using Lua;
using Lua.Unity;

namespace Celeste.Lua
{
    public static class LuaTableExtensions
    {
        public static bool TryGetFunction(this LuaTable table, string functionName, out LuaFunction function)
        {
            if (table.TryGetValue(functionName, out LuaValue value))
            {
                function = value.ReadFunction();
                return function != LuaValue.Nil;
            }
            
            function = default;
            return false;
        }
    }
}