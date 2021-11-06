using Celeste.FSM;
using Celeste.Narrative;
using System;
using XNode;
using XNodeEditor;

namespace CelesteEditor.Narrative
{
    [CustomNodeGraphEditor(typeof(NarrativeGraph))]
    public class NarrativeGraphEditor : NodeGraphEditor
    {
        #region Context Menu

        public override string GetNodeMenuName(Type type)
        {
            return typeof(FSMNode).IsAssignableFrom(type) && !type.IsAbstract ? base.GetNodeMenuName(type) : null;
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
