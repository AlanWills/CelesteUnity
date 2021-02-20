using Celeste.FSM;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM.Nodes
{
    [CustomNodeEditor(typeof(FSMNode))]
    public class FSMNodeEditor : NodeEditor
    {
        #region Properties and Fields

        private static Color SELECTED_COLOUR = new Color(1, 0.5f, 0);

        #endregion

        public sealed override Color GetTint()
        {
            FSMGraph fsmGraph = target.graph as FSMGraph;
            return fsmGraph.startNode == target ? SELECTED_COLOUR : base.GetTint();
        }

        #region GUI Methods

        protected void DrawDefaultPortPair()
        {
            FSMNode fsmNode = target as FSMNode;
            NodeEditorGUILayout.PortPair(
                fsmNode.GetInputPort(FSMNode.DEFAULT_INPUT_PORT_NAME),
                fsmNode.GetOutputPort(FSMNode.DEFAULT_OUTPUT_PORT_NAME));
        }

        #endregion
    }
}
