using Celeste.Objects;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BuildSystem.Data
{
    [CustomEditor(typeof(ScriptingDefineSymbols))]
    public class ScriptingDefineSymbolsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Add Debug Defaults"))
            {
                ScriptingDefineSymbols scriptingDefineSymbols = target as ScriptingDefineSymbols;
                scriptingDefineSymbols.AddItem("INDEX_CHECKS");
                scriptingDefineSymbols.AddItem("NULL_CHECKS");
                scriptingDefineSymbols.AddItem("COMPONENT_CHECKS");
                scriptingDefineSymbols.AddItem("DATA_CHECKS");
                scriptingDefineSymbols.AddItem("KEY_CHECKS");
                scriptingDefineSymbols.AddItem("ALLOCATOR_CHECKS");
            }

            base.OnInspectorGUI();
        }
    }
}
