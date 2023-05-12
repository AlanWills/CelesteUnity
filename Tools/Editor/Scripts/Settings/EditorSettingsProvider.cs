using Celeste.Tools.Settings;
using UnityEditor;

namespace CelesteEditor.Tools.Settings
{
    public class EditorSettingsProvider<T> : SettingsProvider where T : EditorSettings<T>
    {
        #region Properties and Fields

        private Editor editor;

        #endregion

        public EditorSettingsProvider(T settings, string path, SettingsScope scope = SettingsScope.Project)
            : base(path, scope)
        {
            editor = Editor.CreateEditor(settings);
        }

        public override void OnGUI(string searchContext)
        {
            editor.DrawDefaultInspector();
        }
    }
}
