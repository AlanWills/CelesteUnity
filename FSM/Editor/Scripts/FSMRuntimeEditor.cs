using Celeste.FSM;
using Celeste.FSM.Nodes;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(FSMRuntime))]
    public class FSMRuntimeEditor : SceneGraphEditor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            FSMRuntime fsmRuntime = (target as FSMRuntime);
            fsmRuntime.graph = EditorGUILayout.ObjectField(fsmRuntime.graph, typeof(FSMGraph), false) as FSMGraph;

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(fsmRuntime);
            }

            if (fsmRuntime.graph != null && GUILayout.Button("Open graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(fsmRuntime.graph);
            }

            CelesteEditorGUILayout.HorizontalLine();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("lateUpdate"));

            FSMGraphNodePath currentNodeGraphPath = new FSMGraphNodePath(fsmRuntime.CurrentNode);
            EditorGUILayout.LabelField($"Current Node: {currentNodeGraphPath.ReadablePath}");

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
