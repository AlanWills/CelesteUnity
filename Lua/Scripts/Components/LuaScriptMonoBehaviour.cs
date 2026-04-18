#if USE_LUA
using System;
using Celeste.Tools.Attributes.GUI;
using System.Threading.Tasks;
using Lua;
using Lua.Unity;
using Celeste.Lua.Settings;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Lua
{
    public class LuaScriptMonoBehaviour : MonoBehaviour
    {
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
        
        protected virtual void Awake()
        {
            ExecuteLuaFunction(awakeFunctionName).FireAndForget($"{name}.{nameof(Awake)}");
        }
        
        protected virtual void Start()
        {
            ExecuteLuaFunction(startFunctionName).FireAndForget($"{name}.{nameof(Start)}");
        }

        protected virtual void OnEnable()
        {
            ExecuteLuaFunction(onEnableFunctionName).FireAndForget($"{name}.{nameof(OnEnable)}");
        }
        
        protected virtual void OnDisable()
        {
            ExecuteLuaFunction(onDisableFunctionName).FireAndForget($"{name}.{nameof(OnDisable)}");
        }
        
        protected virtual void OnDestroy()
        {
            ExecuteLuaFunction(onDestroyFunctionName).FireAndForget($"{name}.{nameof(OnDestroy)}");
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
    }
}
#endif
