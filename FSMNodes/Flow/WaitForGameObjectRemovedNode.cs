using Celeste.Hierarchy;
using UnityEngine;

namespace Celeste.FSM.Nodes.Flow
{
    [CreateNodeMenu("Celeste/Flow/Wait For GameObject Removed")]
    public class WaitForGameObjectRemovedNode : FSMNode
    {
        #region Properties and Fields

        public GameObjectPath gameObjectPath = new GameObjectPath();
        public float attemptWindow = 1;

        private float currentTime = 0;

        private const string REMOVED_OUTPUT_PORT = "Removed";
        private const string NOT_REMOVED_OUTPUT_PORT = "NotRemoved";

        #endregion

        public WaitForGameObjectRemovedNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(REMOVED_OUTPUT_PORT);
            AddOutputPort(NOT_REMOVED_OUTPUT_PORT);
        }

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            WaitForGameObjectRemovedNode waitForGameObjectRemovedNode = original as WaitForGameObjectRemovedNode;

            attemptWindow = waitForGameObjectRemovedNode.attemptWindow;
            gameObjectPath = new GameObjectPath();
            gameObjectPath.Path = waitForGameObjectRemovedNode.gameObjectPath.Path;
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

                if (gameObjectPath.GameObject == null)
                {
                    Debug.LogFormat("gameObject removed {0}", gameObjectPath.Path);
                    return GetConnectedNode(REMOVED_OUTPUT_PORT);
                }
                else
                {
                    // We are still within the attempt window so stay within this node
                    // Reset cached variables
                    gameObjectPath.Reset();
                    return this;
                }
            }

            Debug.LogFormat("GameObject with path {0} not removed", gameObjectPath);
            return GetConnectedNode(NOT_REMOVED_OUTPUT_PORT);
        }

        #endregion
    }
}
