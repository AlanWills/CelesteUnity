using Celeste.FSM;
using UnityEngine;
using XNodeEditor;

namespace CelesteEditor.FSM.Nodes
{
    [CustomNodeEditor(typeof(FSMNode))]
    public class FSMNodeEditor : NodeEditor
    {
        #region Properties and Fields

        private bool IsCurrentNode
        {
            get { return Application.isPlaying && (target.graph as FSMGraph).Runtime.CurrentNode == target; }
        }

        private static Color START_NODE_COLOUR = new Color(1, 0.5f, 0);
        private static Color FINISH_NODE_COLOUR = new Color(0, 0, 1f);
        private static Color CURRENT_NODE_COLOUR = new Color(1f, 0, 0);

        #endregion

        public sealed override Color GetTint()
        {
            FSMGraph fsmGraph = target.graph as FSMGraph;
            if (fsmGraph.startNode == target)
            {
                return START_NODE_COLOUR;
            }
            else if (fsmGraph is IProgressFSMGraph progressGraph && progressGraph.FinishNode == target)
            {
                return FINISH_NODE_COLOUR;
            }

            return base.GetTint();
        }

        public sealed override Color GetBodyHighlightColour()
        {
            return IsCurrentNode ? CURRENT_NODE_COLOUR : base.GetBodyHighlightColour();
        }

        public sealed override bool ShouldDrawBorder()
        {
            return IsCurrentNode;
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
