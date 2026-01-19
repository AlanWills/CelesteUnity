using System;
using Celeste.Debug.Menus;
using Celeste.Lua.Settings;
using Celeste.Tools;
using Lua;
using Lua.Unity;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaScriptDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM, order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaScriptDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private LuaScript debugMenuLuaScript;
        [SerializeField] private string onShowMenuFunctionName = "onShowMenu";
        [SerializeField] private string onDrawMenuFunctionName = "onDrawMenu";
        [SerializeField] private string onHideMenuFunctionName = "onHideMenu";
        [SerializeField] private LuaRuntime luaRuntime;

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

            debugMenuTable = (await luaRuntime.ExecuteScriptAsync(debugMenuLuaScript)).AsTable();

            onShowMenuFunction = debugMenuTable.GetFunction(onShowMenuFunctionName);
            onDrawMenuFunction = debugMenuTable.GetFunction(onDrawMenuFunctionName);
            onHideMenuFunction = debugMenuTable.GetFunction(onHideMenuFunctionName);
                
            await luaRuntime.ExecuteFunctionAsync(onShowMenuFunction);
        }

        protected override async void OnDrawMenu()
        {
            base.OnDrawMenu();
            
            await luaRuntime.ExecuteFunctionAsync(onDrawMenuFunction);
        }

        protected override async void OnHideMenu()
        {
            base.OnHideMenu();
            
            await luaRuntime.ExecuteFunctionAsync(onHideMenuFunction);
        }
    }
}