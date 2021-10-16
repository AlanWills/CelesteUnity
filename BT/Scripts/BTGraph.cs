using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Celeste.BT
{
    [CreateAssetMenu(fileName = "BTGraph", menuName = "Celeste/BT/BT Graph")]
    public class BTGraph : NodeGraph
    {
        #region Properties and Fields

        public BTNode startNode;

        #endregion

        #region Node Utility Methods

        public override Node AddNode(Type type)
        {
            BTNode node = base.AddNode(type) as BTNode;
            startNode = startNode == null ? node : startNode;
            node.AddToGraph();

            return node;
        }

        public override void RemoveNode(Node node)
        {
            BTNode fsmNode = node as BTNode;
            fsmNode.RemoveFromGraph();

            base.RemoveNode(node);
        }

        public override Node CopyNode(Node original)
        {
            BTNode copy = base.CopyNode(original) as BTNode;
            copy.CopyInGraph(original as BTNode);

            return copy;
        }

        #endregion

        #region Validation Methods

#if UNITY_EDITOR
        public void RemoveNullNodes_EditorOnly()
        {
            nodes.RemoveAll(x => x == null);
        }
#endif

        #endregion
    }
}