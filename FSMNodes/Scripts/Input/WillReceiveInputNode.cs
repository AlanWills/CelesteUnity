using Celeste.Hierarchy;
using Celeste.Input;
using UnityEngine;

namespace Celeste.FSM.Nodes.Input
{
    [CreateNodeMenu("Celeste/Input/Will Receive Input")]
    public class WillReceiveInputNode : FSMNode
    {
        #region Properties and Fields
        
        public GameObjectPath gameObjectPath = new GameObjectPath();
        public float attemptWindow = 1;

        private float currentTime = 0;

        private const string WILL_RECEIVE_INPUT_OUTPUT_PORT = "WillReceiveInput";
        private const string WONT_RECEIVE_INPUT_OUTPUT_PORT = "WontReceiveInput";

        #endregion

        public WillReceiveInputNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(WILL_RECEIVE_INPUT_OUTPUT_PORT);
            AddOutputPort(WONT_RECEIVE_INPUT_OUTPUT_PORT);
        }

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            WillReceiveInputNode willReceiveInput = original as WillReceiveInputNode;

            attemptWindow = willReceiveInput.attemptWindow;
            gameObjectPath = new GameObjectPath();
            gameObjectPath.Path = willReceiveInput.gameObjectPath.Path;
        }

        #endregion

        #region Node Overrides

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
                if (gameObject != null && gameObject.GetComponent<ClickInputListener>() != null && gameObject.GetComponent<ClickInputListener>().enabled)
                {
                    Vector3 inputCentre = gameObject.GetComponent<Collider2D>().bounds.center;
                    GameObject closestGameObject = GetClosestGameObject(inputCentre);

                    if (closestGameObject != null && closestGameObject == gameObject)
                    {
                        Debug.LogFormat("GameObject {0} will receive input", gameObjectPath.Path);
                        return GetConnectedNodeFromOutput(WILL_RECEIVE_INPUT_OUTPUT_PORT);
                    }
                }

                // We are still within the attempt window so stay within this node
                return this;
            }

            Debug.LogFormat("GameObject {0} will not receive input", gameObjectPath);
            return GetConnectedNodeFromOutput(WONT_RECEIVE_INPUT_OUTPUT_PORT);
        }

        #endregion

        #region Utility Functions

        private GameObject GetClosestGameObject(Vector3 position)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(position, Vector2.zero);
            return raycastHit.transform != null ? raycastHit.transform.gameObject : null;
        }

        #endregion
    }
}
