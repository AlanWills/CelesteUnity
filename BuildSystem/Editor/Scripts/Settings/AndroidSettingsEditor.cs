using UnityEditor;

namespace CelesteEditor.BuildSystem
{
    [CustomEditor(typeof(AndroidSettings))]
    [CanEditMultipleObjects]
    public class AndroidSettingsEditor : PlatformSettingsEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            AndroidSettings androidSettings = target as AndroidSettings;
            androidSettings.Architecture = (AndroidArchitecture)EditorGUILayout.EnumFlagsField("Architecture", androidSettings.Architecture);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
