using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lua;
using Lua.IO;
using Lua.Platforms;
using Lua.Standard;
using Lua.Unity;
using Lua.Unity.Interfaces;
using UnityEngine;

namespace Celeste.Lua
{
    [CreateAssetMenu(fileName = nameof(LuaRuntime), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Lua Runtime", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaRuntime : ScriptableObject
    {
#if USE_LUA
        #region Properties and Fields

        public bool IsInitialized => luaState != null;

        [NonSerialized] private LuaState luaState;

        #endregion

        public void Initialize(IReadOnlyList<ILuaLibrary> librariesToOpen, IReadOnlyList<LuaScript> loadOnInitializeScripts)
        {
            var platform = new LuaPlatform(
                FileSystem: new FileSystem(),
                OsEnvironment: new UnityApplicationOsEnvironment(),
                StandardIO: new UnityStandardIO(),
                TimeProvider: TimeProvider.System);
            luaState = LuaState.Create(platform);
            luaState.OpenStandardLibraries();

            foreach (ILuaLibrary luaLibrary in librariesToOpen)
            {
                luaState.OpenLibrary(luaLibrary);
            }
            
#if USE_ADDRESSABLES
            luaState.ModuleLoader = new AddressablesModuleLoader();
#endif

            foreach (LuaScript luaScript in loadOnInitializeScripts)
            {
                ExecuteScriptAsync(luaScript);
            }
        }

        public void Shutdown()
        {
            luaState = null;
        }

        public ValueTask<LuaValue[]> ExecuteScriptAsync(LuaScript luaScript)
        {
            if (!luaScript.TryCompile(luaState))
            {
                UnityEngine.Debug.LogError($"Failed to load lua script '{luaScript.name}' as it has compiler errors.");
                return new ValueTask<LuaValue[]>();
            }
            
#if LUA_EXCEPTION_CHECKS
            try
#endif
            {
                return luaState.DoStringAsync(luaScript.Text);
            }
#if LUA_EXCEPTION_CHECKS
            catch (LuaParseException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua script '{luaScript.name}' as it has a parsing error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaCompileException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua script '{luaScript.name}' as it has a compilation error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaRuntimeException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua script '{luaScript.name}' as it has a runtime error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua script '{luaScript.name}' as it has an unknown error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
#endif
        }

        public ValueTask<LuaValue[]> ExecuteFunctionAsync(LuaFunction luaFunction)
        {
            return ExecuteFunctionAsync(luaFunction, ReadOnlySpan<LuaValue>.Empty);
        }

        public ValueTask<LuaValue[]> ExecuteFunctionAsync(LuaFunction luaFunction, ReadOnlySpan<LuaValue> arguments)
        {
            if (luaFunction == null || luaFunction == LuaValue.Nil)
            {
                return new ValueTask<LuaValue[]>();
            }
            
#if LUA_EXCEPTION_CHECKS
            try
#endif
            {
                return luaState.CallAsync(luaFunction, arguments);
            }
#if LUA_EXCEPTION_CHECKS
            catch (LuaParseException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua function '{luaFunction.Name}' as it has a parsing error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaCompileException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua function '{luaFunction.Name}' as it has a compilation error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaRuntimeException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua function '{luaFunction.Name}' as it has a runtime error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua function '{luaFunction.Name}' as it has an unknown error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
#endif
        }
#endif
    }
}