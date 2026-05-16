#if USE_LUA
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lua;
using Lua.Unity;
#endif

namespace Celeste.Lua
{
    public interface ILuaRuntime
    {
#if USE_LUA
        void Initialize(IReadOnlyList<ILuaLibrary> librariesToOpen, IReadOnlyList<LuaScript> loadOnInitializeScripts);
        void Shutdown();

        ValueTask<LuaValue[]> ExecuteScriptAsync(LuaScript luaScript);
        ValueTask<LuaValue[]> ExecuteFunctionAsync(LuaFunction luaFunction, ReadOnlySpan<LuaValue> arguments);
        
        void OpenLibrary(ILuaLibrary luaLibrary);

        void BindProxy<T, TProxy>(Func<UnityEngine.Object, ILuaProxy> factoryFunc)
            where T : UnityEngine.Object
            where TProxy : ILuaProxy, new();
        bool CanProxy(UnityEngine.Object obj);
        ILuaProxy CreateProxy(UnityEngine.Object obj);
#endif
    }
}