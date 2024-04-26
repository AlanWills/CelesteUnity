using Celeste.BT;
using Celeste.Tools;
using CelesteEditor.Validation.GUIs;
using System;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace CelesteEditor.FSM
{
    [CustomNodeGraphEditor(typeof(BTGraph))]
    public class BTGraphEditor : NodeGraphEditor
    {
        #region Context Menu

        public override string GetNodeMenuName(Type type)
        {
            return typeof(BTNode).IsAssignableFrom(type) ? base.GetNodeMenuName(type) : null;
        }

        #endregion

        #region Add/Remove/Copy

        public override void RemoveNode(Node node)
        {
            base.RemoveNode(node);

            BTGraph btGraph = target as BTGraph;
            if (btGraph.startNode == node && btGraph.nodes.Count > 0)
            {
                btGraph.startNode = btGraph.nodes[0] as BTNode;
            }
        }

        #endregion
    }

    [CustomEditor(typeof(BTGraph))]
    public class BTGraphInspector : GlobalGraphEditor
    {
        #region Properties and Fields

        private ValidatorGUI<BTGraph> btValidatorGUI = new ValidatorGUI<BTGraph>();

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            BTGraph graph = target as BTGraph;

            btValidatorGUI.GUI(graph);

            base.OnInspectorGUI();

            if (GUILayout.Button("Apply Hide Flags"))
            {
                EditorOnly.ApplyHideFlags(graph, HideFlags.HideInHierarchy);
            }

            if (GUILayout.Button("Remove Null Nodes"))
            {
                graph.RemoveNullNodes_EditorOnly();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }

        #endregion
    }
}
