using System.Threading.Tasks;
using Lua;
using Lua.Unity;

namespace Celeste.Lua
{
    public static class LuaRuntimeExtensions
    {
        public static async ValueTask<LuaTable> ExecuteScriptAsClassAsync(this LuaRuntime luaRuntime, LuaScript script)
        {
            var returnValues = (await luaRuntime.ExecuteScriptAsync(script));
            if (returnValues == null || returnValues.Length != 1)
            {
                return default;
            }

            if (returnValues[0].Type != LuaValueType.Table)
            {
                return default;
            }
            
            LuaTable luaTable = returnValues[0].ReadTable();
            return luaTable;
        }
        
        public static async ValueTask<LuaTable> ExecuteScriptAsClassAsync(this LuaRuntime lua, LuaScriptAndVariables scriptAndVariables)
        {
            LuaTable luaTable = await ExecuteScriptAsClassAsync(lua, scriptAndVariables.Script);
            luaTable.InjectVariables(scriptAndVariables.Variables);

            return luaTable;
        }
    }
}