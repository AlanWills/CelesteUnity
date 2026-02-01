#if USE_LUA
using Lua;
using Lua.Standard;

namespace Celeste.Lua
{
    public static class LuaLibraryExtensions
    {
        public static void OpenUnityLibraries(this LuaState state)
        {
            state.OpenGUILayoutLibrary();
        }

        public static void OpenGUILayoutLibrary(this LuaState state)
        {
            state.OpenLibrary(GUILayoutLibrary.Instance);
        }
        
        public static void OpenLibrary(this LuaState state, ILuaLibrary library)
        {
            LuaTable luaTable = new LuaTable(0, library.Functions.Count);
            foreach (LibraryFunction function in library.Functions)
            {
                luaTable[(LuaValue)function.Name] = (LuaValue)function.Func;
            }

            state.Environment[library.Name] = luaTable;
            state.LoadedModules[library.Name] = luaTable;
        }
    }
}
#endif