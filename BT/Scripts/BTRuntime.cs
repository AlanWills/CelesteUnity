using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace Celeste.BT
{
    [AddComponentMenu("Celeste/BT/BT Runtime")]
    public class BTRuntime : SceneGraph<BTGraph>
    {
        public bool Evaluate(BTBlackboard btBlackboard)
        {
            if (graph.startNode == null)
            {
                return false;
            }

            // Iterate through the tree until we find a node which has marked itself as evaluating successfully (by returning null)
            BTNode currentNode = graph.startNode;
            BTNode nextNode = currentNode.Evaluate(btBlackboard);

            while (nextNode != null && currentNode != nextNode)
            {
                // Cycle through the nodes until we either run out or have settled on one
                currentNode = nextNode;
                nextNode = currentNode.Evaluate(btBlackboard);
            }

            // If we have settled we can return true, otherwise we've found no eligible nodes
            return nextNode != null;
        }
    }
}
