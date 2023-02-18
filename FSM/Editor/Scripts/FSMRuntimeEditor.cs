using Celeste;
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
            EditorGUILayout.LabelField($"Current Node: {CurrentNodePath(fsmRuntime)}", GUI.skin.label.New().EnableWrapping());

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        private string CurrentNodePath(ILinearRuntime<FSMNode> fsmRuntime)
        {
            FSMNode currentNode = fsmRuntime.CurrentNode;

            while (currentNode is ILinearRuntime<FSMNode>)
            {
                currentNode = (currentNode as ILinearRuntime<FSMNode>).CurrentNode;
            }

            return new FSMGraphNodePath(currentNode).ReadablePath;
        }
    }
}
