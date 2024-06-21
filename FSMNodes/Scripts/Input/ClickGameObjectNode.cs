using Celeste.Hierarchy;
using Celeste.Utils;
using UnityEngine;

namespace Celeste.FSM.Nodes.Input
{
    [CreateNodeMenu("Celeste/Input/Click GameObject")]
    public class ClickGameObjectNode : FSMNode
    {
        #region Properties and Fields

        public GameObjectPath gameObjectPath = new GameObjectPath();
        public float attemptWindow = 1;

        private float currentTime = 0;

        private const string FOUND_OUTPUT_PORT = "Found";
        private const string NOT_FOUND_OUTPUT_PORT = "NotFound";

        #endregion

        public ClickGameObjectNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(FOUND_OUTPUT_PORT);
            AddOutputPort(NOT_FOUND_OUTPUT_PORT);
        }

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            ClickGameObjectNode clickGameObjectNode = original as ClickGameObjectNode;

            attemptWindow = clickGameObjectNode.attemptWindow;
            gameObjectPath = new GameObjectPath();
            gameObjectPath.Path = clickGameObjectNode.gameObjectPath.Path;
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

                GameObject gameObject = gameObjectPath.GameObject;
                if (gameObject != null)
                {
                    if (gameObject.Click())
                    {
                        Debug.LogFormat("Successfully clicked on {0}", gameObjectPath.Path);
                        return GetConnectedNodeFromOutput(FOUND_OUTPUT_PORT);
                    }
                }

                // We are still within the attempt window so stay within this node
                return this;
            }

            Debug.LogFormat("Failed to click on GameObject with path {0}", gameObjectPath);
            return GetConnectedNodeFromOutput(NOT_FOUND_OUTPUT_PORT);
        }

        #endregion
    }
}
