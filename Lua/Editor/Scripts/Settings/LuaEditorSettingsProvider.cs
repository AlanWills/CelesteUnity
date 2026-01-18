using Celeste.Lua.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Lua.Settings
{
    public class LuaEditorSettingsProvider : EditorSettingsProvider<LuaEditorSettings>
    {
        public LuaEditorSettingsProvider(LuaEditorSettings settings, string path, SettingsScope scope = SettingsScope.Project) 
            : base(settings, path, scope)
        {
        }

        [SettingsProvider]
        public static SettingsProvider CreateLuaEditorSettingsProvider()
        {
            return new LuaEditorSettingsProvider(LuaEditorSettings.GetOrCreateSettings(), "Project/Celeste/Lua Settings")
            {
                keywords = GetSearchKeywordsFromPath(LuaEditorSettings.FILE_PATH)
            };
        }
    }
}