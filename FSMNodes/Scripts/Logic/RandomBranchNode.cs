using System.Collections.Generic;
using XNode;

namespace Celeste.FSM.Nodes.Logic
{
    [CreateNodeMenu("Celeste/Logic/Random Branch")]
    [NodeTint(0.0f, 1, 1)]
    public class RandomBranchNode : FSMNode
    {
        public RandomBranchNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);
        }

        #region FSM Runtime

        protected override FSMNode OnUpdate()
        {
            List<NodePort> outputs = new List<NodePort>(Outputs);
            
            if (outputs.Count == 0)
            {
                return this;
            }

            int randomPort = UnityEngine.Random.Range(0, outputs.Count);
            return GetConnectedNode(outputs[randomPort].fieldName);
        }

        #endregion
    }
}
