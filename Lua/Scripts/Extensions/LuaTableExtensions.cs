using System.Collections.Generic;
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

        public static void InjectVariables(this LuaTable table, IReadOnlyList<ScriptVariable> variables)
        {
            if (table == null || table == LuaValue.Nil)
            {
                return;
            }
            
            foreach (var variable in variables)
            {
                table.SetValue(variable.Name, LuaValue.FromObject(variable.Value));
            }
        }
    }
}