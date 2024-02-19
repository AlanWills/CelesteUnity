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
            narrativeRuntime.graph = EditorGUILayout.ObjectField("Graph", narrativeRuntime.graph, typeof(NarrativeGraph), false) as NarrativeGraph;

            DrawPropertiesExcluding(serializedObject, nameof(narrativeRuntime.graph), "m_Script");

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(narrativeRuntime);
            }

            if (narrativeRuntime.graph != null && GUILayout.Button("Open graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(narrativeRuntime.graph);
            }

            if (Application.isPlaying)
            {
                CelesteEditorGUILayout.HorizontalLine();

                EditorGUILayout.LabelField($"Current Node: {(narrativeRuntime.CurrentNode != null ? narrativeRuntime.CurrentNode.name : "null")}");
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
