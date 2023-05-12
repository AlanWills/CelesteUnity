using Celeste.Sound.Settings;
using CelesteEditor.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Localisation.Settings
{
    public class SoundEditorSettingsProvider : EditorSettingsProvider<SoundEditorSettings>
    {
        public SoundEditorSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            : base(SoundEditorSettings.GetOrCreateSettings(), path, scope) { }
        
        [SettingsProvider]
        public static SettingsProvider CreateSoundSettingsProvider()
        {
            return new SoundEditorSettingsProvider("Project/Celeste/Sound Settings", SettingsScope.Project)
            {
                keywords = GetSearchKeywordsFromPath(SoundEditorSettings.FILE_PATH)
            };
        }
    }
}