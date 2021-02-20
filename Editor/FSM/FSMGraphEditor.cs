using Celeste.FSM;
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

namespace CelesteEditor.FSM
{
    [CustomNodeGraphEditor(typeof(FSMGraph))]
    public class FSMGraphEditor : NodeGraphEditor
    {
        #region Context Menu

        public override string GetNodeMenuName(Type type)
        {
            return typeof(FSMNode).IsAssignableFrom(type) ? base.GetNodeMenuName(type) : null;
        }

        #endregion

        #region Add/Remove/Copy

        public override void RemoveNode(Node node)
        {
            base.RemoveNode(node);

            if (target.nodes.Count == 1)
            {
                (target as FSMGraph).startNode = target.nodes[0] as FSMNode;
            }
        }

        #endregion
    }

    [CustomEditor(typeof(FSMGraph))]
    public class FSMGraphInspector : GlobalGraphEditor
    {
        #region Properties and Fields

        private ScriptableObject removeAsset;
        private ValidatorGUI<FSMGraph> fsmValidatorGUI = new ValidatorGUI<FSMGraph>();

        #endregion

        #region Editor Methods

        public override void OnInspectorGUI()
        {
            FSMGraph graph = target as FSMGraph;

            fsmValidatorGUI.GUI(graph);

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

            EditorGUILayout.BeginHorizontal();

            removeAsset = EditorGUILayout.ObjectField(removeAsset, typeof(ScriptableObject), false) as ScriptableObject;

            if (GUILayout.Button("Remove Asset"))
            {
                graph.RemoveAsset(removeAsset);
                EditorUtility.SetDirty(target);
                
                removeAsset = null;
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
