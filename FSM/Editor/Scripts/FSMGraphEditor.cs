using Celeste.FSM;
using System;
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
}
