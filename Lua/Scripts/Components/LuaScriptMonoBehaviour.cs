using System;
using Celeste.Tools.Attributes.GUI;
#if USE_LUA
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

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField] private LuaScriptAndVariables script;
        [SerializeField] private bool showAdvancedSettings;
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string awakeFunctionName = "awake";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string startFunctionName = "start";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onEnableFunctionName = "onEnable";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onDisableFunctionName = "onDisable";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onDestroyFunctionName = "onDestroy";

        [NonSerialized] private LuaTable componentTable;
        [NonSerialized] private bool scriptRun;

        #endregion
        
        #region Unity Methods

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (luaRuntime == null)
            {
                luaRuntime = LuaEditorSettings.GetOrCreateSettings().LuaRuntime;
            }
#endif
        }
        
        private async void Awake()
        {
            RunScriptIfNecessary();
            
            LuaFunction awakeFunction = componentTable.GetFunction(awakeFunctionName);
            await luaRuntime.ExecuteFunctionAsync(awakeFunction, componentTable);
        }
        
        private async void Start()
        {
            RunScriptIfNecessary();
            
            LuaFunction startFunction = componentTable.GetFunction(startFunctionName);
            await luaRuntime.ExecuteFunctionAsync(startFunction, componentTable);
        }

        private async void OnEnable()
        {
            RunScriptIfNecessary();
            
            LuaFunction onEnableFunction = componentTable.GetFunction(onEnableFunctionName);
            await luaRuntime.ExecuteFunctionAsync(onEnableFunction, componentTable);
        }
        
        private async void OnDisable()
        {
            RunScriptIfNecessary();
            
            LuaFunction onDisableFunction = componentTable.GetFunction(onDisableFunctionName);
            await luaRuntime.ExecuteFunctionAsync(onDisableFunction, componentTable);
        }
        
        private async void OnDestroy()
        {
            RunScriptIfNecessary();
            
            LuaFunction onDestroyFunction = componentTable.GetFunction(onDestroyFunctionName);
            await luaRuntime.ExecuteFunctionAsync(onDestroyFunction, componentTable);
        }
        
        #endregion

        private async void RunScriptIfNecessary()
        {
            if (scriptRun)
            {
                return;
            }
            
            scriptRun = true;
            componentTable = await luaRuntime.ExecuteScriptAsClassAsync(script);
        }
#endif
    }
}