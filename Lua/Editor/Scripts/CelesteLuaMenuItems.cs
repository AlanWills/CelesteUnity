#if USE_LUA
using System.IO;
using Celeste;
using Celeste.Tools;
using UnityEditor;
using UnityEngine;

namespace Lua.Unity.Editor
{
    public static class LuaMenuItems
    {
        [MenuItem("Assets/Create/" + CelesteMenuItemConstants.LUA_MENU_ITEM + "New Lua Script", priority = CelesteMenuItemConstants.LUA_MENU_ITEM_PRIORITY)]
        public static void CreateNewLuaScript()
        {
            string currentlySelectedObjectFolder = EditorOnly.GetSelectionObjectPath();
            if (!AssetDatabase.IsValidFolder(currentlySelectedObjectFolder))
            {
                currentlySelectedObjectFolder = Path.GetDirectoryName(currentlySelectedObjectFolder);
            }

            string newScriptPath = Path.Combine(currentlySelectedObjectFolder, "New Lua Script.lua");
            File.WriteAllText(newScriptPath, string.Empty);
            AssetDatabase.Refresh();

            Object luaScriptObject = AssetDatabase.LoadAssetAtPath<Object>(newScriptPath);
            Selection.activeObject = luaScriptObject;
            EditorGUIUtility.PingObject(luaScriptObject);
        }
    }
}
#endif
