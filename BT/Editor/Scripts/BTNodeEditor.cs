using Celeste.BT;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.BT.Nodes
{
    [CustomNodeEditor(typeof(BTNode))]
    public class BTNodeEditor : NodeEditor
    {
        #region Properties and Fields

        private static Color SELECTED_COLOUR = new Color(1, 0.5f, 0);

        #endregion

        public sealed override Color GetTint()
        {
            BTGraph btGraph = target.graph as BTGraph;
            return btGraph.startNode == target ? SELECTED_COLOUR : base.GetTint();
        }

        #region GUI Methods

        protected void DrawDefaultPortPair()
        {
            BTNode btNode = target as BTNode;
            NodeEditorGUILayout.PortPair(
                btNode.GetInputPort(BTNode.DEFAULT_INPUT_PORT_NAME),
                btNode.GetOutputPort(BTNode.DEFAULT_OUTPUT_PORT_NAME));
        }

        #endregion
    }
}
