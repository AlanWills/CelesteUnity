using UnityEngine;
#if USE_LUA
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lua;
using Lua.Standard;
using Lua.Unity;

namespace Celeste.Lua
{
    [CreateAssetMenu(fileName = nameof(LuaRuntime), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Lua Runtime", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaRuntime : ScriptableObject, ILuaRuntime
    {
        #region Properties and Fields

        public bool IsInitialized { get; private set; }

        [NonSerialized] private LuaState luaState;
        [NonSerialized] private readonly Dictionary<Type, Func<UnityEngine.Object, ILuaProxy>> proxies = new();

        #endregion

        public async ValueTask InitializeAsync(IReadOnlyList<ILuaLibrary> librariesToOpen, IReadOnlyList<LuaScript> loadOnInitializeScripts)
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
                await ExecuteScriptAsync(luaScript);
            }
            
            IsInitialized = true;
        }

        public void Shutdown()
        {
            IsInitialized = false;
            luaState = null;
        }

        public ValueTask<LuaValue[]> ExecuteScriptAsync(LuaScript luaScript)
        {
            if (string.IsNullOrWhiteSpace(luaScript.Text))
            {
                return new ValueTask<LuaValue[]>();
            }
            
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
        public ValueTask<LuaValue[]> ExecuteStringAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValueTask<LuaValue[]>();
            }
            
#if LUA_EXCEPTION_CHECKS
            try
#endif
            {
                return luaState.DoStringAsync(text);
            }
#if LUA_EXCEPTION_CHECKS
            catch (LuaParseException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua string as it has a parsing error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaCompileException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua string as it has a compilation error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (LuaRuntimeException e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua string as it has a runtime error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to execute lua string as it has an unknown error: {e.Message}.");
                return new ValueTask<LuaValue[]>();
            }
#endif
        }
        public void OpenLibrary(ILuaLibrary luaLibrary)
        {
            luaState.OpenLibrary(luaLibrary);
            luaLibrary.LuaRuntime = this;
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
            UnityEngine.Debug.Assert(value != null, $"Attempting to set environment variable '{variableName}' to a null value.");
            LuaValue luaValue = LuaValue.FromObject(value);
            luaState.Environment[variableName] = luaValue;
        }

        public void ClearEnvironmentVariable(string variableName)
        {
            luaState?.Environment.SetNil(variableName);
        }
    }
}
#else
namespace Celeste.Lua
{
    public class LuaRuntime : ScriptableObject, ILuaRuntime
    {
    }
}
#endif
