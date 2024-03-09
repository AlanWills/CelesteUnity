using Celeste;
using Celeste.FSM;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    [CustomEditor(typeof(FSMRuntime))]
    public class FSMRuntimeEditor : ILinearRuntimeEditor<FSMGraph> { }

    public class ILinearRuntimeEditor<TGraph> : SceneGraphEditor 
        where TGraph : FSMGraph 
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            ILinearRuntime runtime = target as ILinearRuntime;
            SceneGraph sceneGraph = target as SceneGraph;
            sceneGraph.graph = EditorGUILayout.ObjectField(sceneGraph.graph, typeof(TGraph), false) as TGraph;

            DrawPropertiesExcluding(serializedObject, nameof(sceneGraph.graph), "m_Script");

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            if (sceneGraph.graph != null && GUILayout.Button("Open graph", GUILayout.Height(40)))
            {
                NodeEditorWindow.Open(sceneGraph.graph);
            }

            if (Application.isPlaying)
            {
                CelesteEditorGUILayout.HorizontalLine();
                EditorGUILayout.LabelField($"Current Node: {CurrentNodePath(runtime)}", GUI.skin.label.New().EnableWrapping());
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        private string CurrentNodePath(ILinearRuntime fsmRuntime)
        {
            FSMNode currentNode = fsmRuntime.CurrentNode;

            while (currentNode is ILinearRuntime)
            {
                currentNode = (currentNode as ILinearRuntime).CurrentNode;
            }

            return new FSMGraphNodePath(currentNode).ReadablePath;
        }
    }
}
