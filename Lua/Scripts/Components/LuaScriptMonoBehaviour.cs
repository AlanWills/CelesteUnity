using System;
using Celeste.Tools.Attributes.GUI;
#if USE_LUA
using System.Threading.Tasks;
using Lua;
using Lua.Unity;
using Celeste.Lua.Settings;
#endif
using UnityEngine;

namespace Celeste.Lua
{
    public class LuaScriptMonoBehaviour : MonoBehaviour
    {
#if USE_LUA
        #region Properties and Fields

        protected LuaRuntime LuaRuntime => luaRuntime;
        protected LuaTable ComponentTable => componentTable;
        
        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField, InlineDataInInspector] private LuaScriptAndVariables script;
        [SerializeField] protected bool showAdvancedSettings;
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string awakeFunctionName = "awake";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string startFunctionName = "start";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onEnableFunctionName = "onEnable";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onDisableFunctionName = "onDisable";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onDestroyFunctionName = "onDestroy";

        [NonSerialized] private LuaTable componentTable;
        [NonSerialized] private bool scriptRun;

        #endregion
        
        #region Unity Methods

        protected virtual void OnValidate()
        {
#if UNITY_EDITOR
            if (luaRuntime == null)
            {
                luaRuntime = LuaEditorSettings.GetOrCreateSettings().LuaRuntime;
            }
#endif
        }
        
        protected virtual async void Awake()
        {
            await ExecuteLuaFunction(awakeFunctionName);
        }
        
        protected virtual async void Start()
        {
            await ExecuteLuaFunction(startFunctionName);
        }

        protected virtual async void OnEnable()
        {
            await ExecuteLuaFunction(onEnableFunctionName);
        }
        
        protected virtual async void OnDisable()
        {
            await ExecuteLuaFunction(onDisableFunctionName);
        }
        
        protected virtual async void OnDestroy()
        {
            await ExecuteLuaFunction(onDestroyFunctionName);
        }
        
        #endregion

        protected async ValueTask<LuaValue[]> ExecuteLuaFunction(string functionName)
        {
            await RunScriptIfNecessary();
            
            LuaFunction function = componentTable.GetFunction(functionName);
            return await luaRuntime.ExecuteFunctionAsync(function, componentTable);
        }

        private async ValueTask<LuaTable> RunScriptIfNecessary()
        {
            if (scriptRun)
            {
                return componentTable;
            }
            
            scriptRun = true;
            componentTable = await luaRuntime.ExecuteScriptAsClassAsync(script);
            return componentTable;
        }
#endif
    }
}