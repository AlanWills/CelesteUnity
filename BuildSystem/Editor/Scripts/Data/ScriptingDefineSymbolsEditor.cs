using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CustomEditor(typeof(ScriptingDefineSymbols))]
    public class ScriptingDefineSymbolsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Add Debug Defaults"))
                {
                    ScriptingDefineSymbols scriptingDefineSymbols = target as ScriptingDefineSymbols;
                    scriptingDefineSymbols.AddDefaultDebugSymbols();
                }

                if (GUILayout.Button("Add Release Defaults"))
                {
                    ScriptingDefineSymbols scriptingDefineSymbols = target as ScriptingDefineSymbols;
                    scriptingDefineSymbols.AddDefaultReleaseSymbols();
                }
            }

            base.OnInspectorGUI();
        }
    }
}
