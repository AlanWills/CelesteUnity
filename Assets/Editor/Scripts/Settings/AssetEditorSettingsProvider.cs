using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Assets.Settings
{
    public class AssetEditorSettingsProvider : EditorSettingsProvider<AssetEditorSettings>
    {
        private AssetEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(AssetEditorSettings.GetOrCreateSettings(), path, scope) { }

        [SettingsProvider]
        public static SettingsProvider CreateAssetSettingsProvider()
        {
            return new AssetEditorSettingsProvider("Project/Celeste/Asset Settings")
            {
                keywords = GetSearchKeywordsFromPath(AssetEditorSettings.FILE_PATH)
            };
        }
    }
}