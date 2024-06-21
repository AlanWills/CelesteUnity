using Celeste.Hierarchy;
using UnityEngine;

namespace Celeste.FSM.Nodes.Flow
{
    [CreateNodeMenu("Celeste/Flow/Wait For GameObject")]
    public class WaitForGameObjectNode : FSMNode
    {
        #region Properties and Fields

        public GameObjectPath gameObjectPath = new GameObjectPath();
        public float attemptWindow = 1;

        private float currentTime = 0;

        private const string FOUND_OUTPUT_PORT = "Found";
        private const string NOT_FOUND_OUTPUT_PORT = "NotFound";

        #endregion

        public WaitForGameObjectNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(FOUND_OUTPUT_PORT);
            AddOutputPort(NOT_FOUND_OUTPUT_PORT);
        }

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            WaitForGameObjectNode waitForGameObjectNode = original as WaitForGameObjectNode;

            attemptWindow = waitForGameObjectNode.attemptWindow;
            gameObjectPath = new GameObjectPath();
            gameObjectPath.Path = waitForGameObjectNode.gameObjectPath.Path;
        }

        #endregion

        #region FSM Runtime

        protected override void OnEnter()
        {
            base.OnEnter();

            currentTime = 0;
            gameObjectPath.Reset();
        }

        protected override FSMNode OnUpdate()
        {
            if (currentTime <= attemptWindow)
            {
                currentTime += Time.deltaTime;

                if (gameObjectPath.GameObject != null)
                {
                    Debug.LogFormat("Found GameObject {0}", gameObjectPath.Path);
                    return GetConnectedNodeFromOutput(FOUND_OUTPUT_PORT);
                }
                else
                {
                    // We are still within the attempt window so stay within this node
                    return this;
                }
            }

            Debug.LogFormat("Could not find GameObject with path {0}", gameObjectPath);
            return GetConnectedNodeFromOutput(NOT_FOUND_OUTPUT_PORT);
        }

        #endregion
    }
}
