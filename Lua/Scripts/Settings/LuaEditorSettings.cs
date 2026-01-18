using Celeste.Tools.Settings;
using UnityEngine;

namespace Celeste.Lua.Settings
{
#if UNITY_EDITOR && USE_LUA
    public class LuaEditorSettings : EditorSettings<LuaEditorSettings>
    {
        #region Properties and Fields
        
        public const string FOLDER_PATH = "Assets/Editor/Lua/";
        public const string FILE_PATH = FOLDER_PATH + "LuaEditorSettings.asset";

        public LuaRuntime LuaRuntime => luaRuntime;
        
        [SerializeField] private LuaRuntime luaRuntime;
        
        #endregion

        public static LuaEditorSettings GetOrCreateSettings()
        {
            return GetOrCreateSettings(FOLDER_PATH, FILE_PATH);
        }
    }
#endif
}