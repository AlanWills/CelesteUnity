using Celeste.FSM;
using Celeste.FSM.Utils;
using Celeste.Narrative;
using CelesteEditor.Tools;
using CelesteEditor.Validation.GUIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CelesteEditor.Narrative
{
    [CustomEditor(typeof(NarrativeGraph))]
    public class NarrativeGraphInspector : GlobalGraphEditor
    {
        #region Properties and Fields

        private FSMGraph fsmGraph;
        private ScriptableObject removeAsset;
        private ValidatorGUI<NarrativeGraph> narrativeValidatorGUI = new ValidatorGUI<NarrativeGraph>();

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            NarrativeGraph graph = target as NarrativeGraph;

            narrativeValidatorGUI.GUI(graph);

            base.OnInspectorGUI();

            if (GUILayout.Button("Apply Hide Flags"))
            {
                AssetUtility.ApplyHideFlags(graph, HideFlags.HideInHierarchy);
            }

            if (GUILayout.Button("Remove Null Nodes"))
            {
                graph.RemoveNullNodes_EditorOnly();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                fsmGraph = EditorGUILayout.ObjectField(fsmGraph, typeof(FSMGraph), false) as FSMGraph;

                if (GUILayout.Button("Import Nodes", GUILayout.ExpandWidth(false)))
                {
                    var editor = NodeGraphEditor.GetEditor(graph, NodeEditorWindow.current);
                    foreach (var node in fsmGraph.nodes)
                    {
                        var copy = editor.CopyNode(node);
                        copy.name = node.name;

                        EditorUtility.SetDirty(target);
                    }
                }
            };

            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                removeAsset = EditorGUILayout.ObjectField(removeAsset, typeof(ScriptableObject), false) as ScriptableObject;

                if (GUILayout.Button("Remove Asset", GUILayout.ExpandWidth(false)))
                {
                    graph.RemoveAsset(removeAsset);
                    EditorUtility.SetDirty(target);

                    removeAsset = null;
                }
            };
        }

        #endregion
    }
}
