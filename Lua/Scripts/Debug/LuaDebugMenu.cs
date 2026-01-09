#if USE_LUA
using System;
using Celeste.Debug.Menus;
using Lua;
using Lua.IO;
using Lua.Platforms;
using Lua.Standard;
using Lua.Unity;
using UnityEngine;

namespace Celeste.Lua.Debug
{
    [CreateAssetMenu(fileName = nameof(LuaDebugMenu), menuName = CelesteMenuItemConstants.LUA_MENU_ITEM + "Debug/Lua Debug Menu", order = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
    public class LuaDebugMenu : DebugMenu
    {
        private LuaState state;
        
        protected override void OnShowMenu()
        {
            base.OnShowMenu();
            
            var platform = new LuaPlatform(
                fileSystem: new FileSystem(),
                osEnvironment: new UnityApplicationOsEnvironment(),
                standardIO: new UnityStandardIO(),
                timeProvider: TimeProvider.System);
            state = LuaState.Create(platform);
            state.OpenStandardLibraries();
            state.OpenUnityLibraries();
            state.ModuleLoader = new AddressablesModuleLoader();
        }

        protected override async void OnDrawMenu()
        {
            await state.DoStringAsync("if GUILayout.button(\"Test\") then print(\"Wubba Lubba Dub Dub\") end");
        }
    }
}
#endif