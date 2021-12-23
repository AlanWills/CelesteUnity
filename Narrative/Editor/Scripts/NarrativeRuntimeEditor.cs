using Celeste.FSM;
using Celeste.Narrative;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(NarrativeRuntime))]
    public class NarrativeRuntimeEditor : SceneGraphEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            NarrativeRuntime narrativeRuntime = target as NarrativeRuntime;
            narrativeRuntime.graph = EditorGUILayout.ObjectField(narrativeRuntime.graph, typeof(NarrativeGraph), false) as NarrativeGraph;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(narrativeRuntime);
            }

            if (narrativeRuntime.graph != null && GUILayout.Button("Open graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(narrativeRuntime.graph);
            }

            CelesteEditorGUILayout.HorizontalLine();

            EditorGUILayout.LabelField($"Current Node: {(narrativeRuntime.CurrentNode != null ? narrativeRuntime.CurrentNode.name : "null")}");

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
