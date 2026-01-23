using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lua;
using Lua.Standard;
using Lua.Unity;
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
        [NonSerialized] private readonly Dictionary<Type, Func<UnityEngine.Object, ILuaProxy>> proxies = new();

        #endregion

        public void Initialize(IReadOnlyList<ILuaLibrary> librariesToOpen, IReadOnlyList<LuaScript> loadOnInitializeScripts)
        {
            luaState = LuaUtility.CreateUnityLuaState();
            luaState.OpenStandardLibraries();

            foreach (ILuaLibrary luaLibrary in librariesToOpen)
            {
                OpenLibrary(luaLibrary);
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

        public ValueTask<LuaValue[]> ExecuteFunctionAsync(LuaFunction luaFunction, LuaValue argument)
        {
            return ExecuteFunctionAsync(luaFunction, new[] { argument });
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

        public void OpenLibrary(ILuaLibrary luaLibrary)
        {
            luaState.OpenLibrary(luaLibrary);
        }

        public void BindProxy<T, TProxy>(Func<UnityEngine.Object, ILuaProxy> factoryFunc) 
            where T : UnityEngine.Object 
            where TProxy : ILuaProxy, new()
        {
            TProxy proxy = new TProxy();
            SetEnvironmentVariable(proxy.Name, proxy);
            proxies[typeof(T)] = factoryFunc;
        }
        
        public bool CanProxy(UnityEngine.Object obj)
        {
            return proxies.ContainsKey(obj.GetType());
        }

        public ILuaProxy CreateProxy(UnityEngine.Object obj)
        {
            if (proxies.TryGetValue(obj.GetType(), out var factoryFunc))
            {
                return factoryFunc(obj);
            }

            return null;
        }

        public void SetEnvironmentVariable<T>(string variableName, T value)
        {
            luaState.Environment[variableName] = LuaValue.FromObject(value);
        }
#endif
    }
}