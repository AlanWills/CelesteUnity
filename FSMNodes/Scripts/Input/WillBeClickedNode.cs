using Celeste.Hierarchy;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Celeste.FSM.Nodes.Input
{
    [CreateNodeMenu("Celeste/Input/Will Be Clicked")]
    public class WillBeClickedNode : FSMNode
    {
        #region Properties and Fields
        
        public GameObjectPath gameObjectPath = new GameObjectPath();
        public float attemptWindow = 1;

        private float currentTime = 0;

        private const string WILL_BE_CLICKED_OUTPUT_PORT = "WillBeClicked";
        private const string WONT_BE_CLICKED_OUTPUT_PORT = "WontBeClicked";

        #endregion

        public WillBeClickedNode()
        {
            RemoveDynamicPort(DEFAULT_OUTPUT_PORT_NAME);

            AddOutputPort(WILL_BE_CLICKED_OUTPUT_PORT);
            AddOutputPort(WONT_BE_CLICKED_OUTPUT_PORT);
        }

        #region Add/Remove/Copy

        protected override void OnCopyInGraph(FSMNode original)
        {
            base.OnCopyInGraph(original);

            WillBeClickedNode willBeClicked = original as WillBeClickedNode;

            attemptWindow = willBeClicked.attemptWindow;
            gameObjectPath = new GameObjectPath();
            gameObjectPath.Path = willBeClicked.gameObjectPath.Path;
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
                if (gameObject != null)
                {
                    GameObject closestGameObject = GetClosestGameObject(gameObject.transform.position);
                    if (closestGameObject != null && closestGameObject == ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject))
                    {
                        Debug.LogFormat("GameObject {0} will be clicked", gameObjectPath.Path);
                        return GetConnectedNodeFromOutput(WILL_BE_CLICKED_OUTPUT_PORT);
                    }
                }

                // We are still within the attempt window so stay within this node
                return this;
            }

            Debug.LogFormat("GameObject {0} will not be clicked", gameObjectPath);
            return GetConnectedNodeFromOutput(WONT_BE_CLICKED_OUTPUT_PORT);
        }

        #endregion

        #region Utility Functions

        private GameObject GetClosestGameObject(Vector3 position)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            GameObject closestGameObject = null;
            float closestDistance = float.MaxValue;
            int closestDepth = int.MaxValue;

            foreach (RaycastResult result in results)
            {
                if (result.distance < closestDistance ||
                    (result.distance == closestDistance && result.depth > closestDepth))
                {
                    closestGameObject = result.gameObject;
                    closestDistance = result.distance;
                    closestDepth = result.depth;
                }
            }

            return closestGameObject;
        }

        #endregion
    }
}
