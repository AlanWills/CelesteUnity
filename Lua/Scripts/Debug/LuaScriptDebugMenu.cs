#if USE_LUA
using System;
using Celeste.Debug.Menus;
using Celeste.Lua.Settings;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaScriptDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Debug/Lua Script Debug Menu", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaScriptDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private LuaRuntime luaRuntime;
        [SerializeField, InlineDataInInspector] private LuaScriptAndVariables script;
        [SerializeField] private bool showAdvancedSettings;
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onShowMenuFunctionName = "onShowMenu";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onDrawMenuFunctionName = "onDrawMenu";
        [SerializeField, ShowIf(nameof(showAdvancedSettings))] private string onHideMenuFunctionName = "onHideMenu";

        [NonSerialized] private LuaTable debugMenuTable;
        [NonSerialized] private LuaFunction onShowMenuFunction;
        [NonSerialized] private LuaFunction onDrawMenuFunction;
        [NonSerialized] private LuaFunction onHideMenuFunction;

        #endregion

        #region Unity Methods

        protected override void OnValidate()
        {
            base.OnValidate();

#if UNITY_EDITOR
            if (luaRuntime == null)
            {
                luaRuntime = LuaEditorSettings.GetOrCreateSettings().LuaRuntime;
                EditorOnly.SetDirty(this);
            }
#endif
        }
        
        #endregion

        protected override async void OnShowMenu()
        {
            base.OnShowMenu();
            
            debugMenuTable = await luaRuntime.ExecuteScriptAsClassAsync(script);

            onShowMenuFunction = debugMenuTable.GetFunction(onShowMenuFunctionName);
            onDrawMenuFunction = debugMenuTable.GetFunction(onDrawMenuFunctionName);
            onHideMenuFunction = debugMenuTable.GetFunction(onHideMenuFunctionName);

            luaRuntime.ExecuteFunctionAsync(onShowMenuFunction, debugMenuTable).FireAndForget();
        }

        protected override void OnDrawMenu()
        {
            base.OnDrawMenu();

            luaRuntime.ExecuteFunctionAsync(onDrawMenuFunction, debugMenuTable).FireAndForget();
        }

        protected override void OnHideMenu()
        {
            base.OnHideMenu();

            luaRuntime.ExecuteFunctionAsync(onHideMenuFunction, debugMenuTable).FireAndForget();
        }
    }
}
#endif