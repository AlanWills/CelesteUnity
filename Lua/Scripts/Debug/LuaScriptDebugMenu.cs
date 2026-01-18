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
        [SerializeField] private LuaRuntime luaRuntime;

        [NonSerialized] private LuaTable debugMenuTable;

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

            if (debugMenuTable.TryGetValue("OnShowMenu", out var value))
            {
                LuaFunction onShowMenuFunction = value.ReadFunction();
                
                if (onShowMenuFunction != null)
                {
                    //onShowMenuFunction.Func.Invoke();
                }
            }
        }

        protected override void OnDrawMenu()
        {
            base.OnDrawMenu();
        }

        protected override void OnHideMenu()
        {
            base.OnHideMenu();
        }
    }
}