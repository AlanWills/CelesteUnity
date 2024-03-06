using Celeste.FSM;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    public class FSMGraphInspectorUtils
    {
        private FSMGraph fsmGraph;
        private ScriptableObject removeAsset;

        public void GUI(FSMGraph graph)
        {
            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                if (graph.startNode != null && GUILayout.Button("Focus On Start"))
                {
                    NodeEditorWindow.current.panOffset = graph.startNode.position;
                }

                if (GUILayout.Button("Apply Hide Flags"))
                {
                    AssetUtility.ApplyHideFlags(graph, HideFlags.HideInHierarchy);
                }

                if (GUILayout.Button("Remove Hide Flags"))
                {
                    AssetUtility.RemoveHideFlags(graph, HideFlags.HideInHierarchy);
                }
            }

            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                fsmGraph = EditorGUILayout.ObjectField(fsmGraph, typeof(FSMGraph), false) as FSMGraph;

                if (GUILayout.Button("Import Nodes", GUILayout.ExpandWidth(false)))
                {
                    foreach (var node in fsmGraph.nodes)
                    {
                        var copiedNode = graph.CopyNode(node);
                        AssetDatabase.AddObjectToAsset(copiedNode, graph);
                        EditorUtility.SetDirty(graph);
                    }
                }
            }

            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                removeAsset = EditorGUILayout.ObjectField(removeAsset, typeof(ScriptableObject), false) as ScriptableObject;

                if (GUILayout.Button("Remove Asset", GUILayout.ExpandWidth(false)))
                {
                    graph.RemoveAsset(removeAsset);
                    EditorUtility.SetDirty(graph);

                    removeAsset = null;
                }
            }
        }
    }
}