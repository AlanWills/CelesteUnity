using Celeste.FSM;
using Celeste.FSM.Utils;
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
    [CustomEditor(typeof(FSMGraph))]
    public class FSMGraphInspector : Editor
    {
        #region Properties and Fields

        private FSMGraph fsmGraph;
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

            using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
            {
                fsmGraph = EditorGUILayout.ObjectField(fsmGraph, typeof(FSMGraph), false) as FSMGraph;

                if (GUILayout.Button("Import Nodes", GUILayout.ExpandWidth(false)))
                {
                    foreach (var node in fsmGraph.nodes)
                    {
                        graph.CopyNode(node);
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
