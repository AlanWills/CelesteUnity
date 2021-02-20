using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Event = Celeste.Events.Event;

namespace Celeste.Input
{
    [AddComponentMenu("Celeste/Input/Input Manager")]
    public class InputManager : MonoBehaviour
    {
        #region Properties and Fields

        public Camera raycastCamera;

        #region Desktop Variables

        [Header("Desktop Events)")]
        public Vector3Event leftMouseButtonFirstDown;
        public Vector3Event leftMouseButtonDown;
        public Vector3Event leftMouseButtonFirstUp;

        public Vector3Event middleMouseButtonFirstDown;
        public Vector3Event middleMouseButtonDown;
        public Vector3Event middleMouseButtonFirstUp;

        public Vector3Event rightMouseButtonFirstDown;
        public Vector3Event rightMouseButtonDown;
        public Vector3Event rightMouseButtonFirstUp;

        public FloatEvent mouseScrolled;

        #endregion

        #region Phone Variables

        [Header("Phone Events")]
        public TouchEvent singleTouchEvent;
        public MultiTouchEvent doubleTouchEvent;
        public MultiTouchEvent tripleTouchEvent;

        #endregion

        #region Common Variables

        [Header("Common Events")]
        public GameObjectClickEvent gameObjectLeftClicked;

        private GameObject firstHitGameObject;
        private Vector3 firstHitGameObjectPosition;

        #endregion

        #endregion

        #region Unity Methods

        public void Update()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (UnityEngine.Input.touchCount != 1)
            {
                firstHitGameObject = null;
            }

            if (UnityEngine.Input.touchCount == 1)
            {
                Touch touch = UnityEngine.Input.GetTouch(0);
                singleTouchEvent.RaiseSilently(touch);

                Vector3 touchWorldPosition = raycastCamera.ScreenToWorldPoint(touch.position);
                GameObject hitGameObject = Raycast(new Vector2(touchWorldPosition.x, touchWorldPosition.y));

                if (touch.phase == TouchPhase.Began)
                {
                    firstHitGameObject = hitGameObject;
                    firstHitGameObjectPosition = touchWorldPosition;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (firstHitGameObject != null && 
                        firstHitGameObject == hitGameObject && 
                        (touchWorldPosition - firstHitGameObjectPosition).sqrMagnitude < 0.05f)
                    {
                        gameObjectLeftClicked.Raise(new GameObjectClickEventArgs()
                        {
                            gameObject = hitGameObject,
                            clickWorldPosition = touchWorldPosition
                        });
                    }

                    firstHitGameObject = null;
                }
            }
            else if (UnityEngine.Input.touchCount == 2)
            {
                doubleTouchEvent.RaiseSilently(new MultiTouchEventArgs()
                {
                    touchCount = 2,
                    touches = UnityEngine.Input.touches,
                });
            }
            else if (UnityEngine.Input.touchCount == 3)
            {
                tripleTouchEvent.RaiseSilently(new MultiTouchEventArgs()
                {
                    touchCount = 3,
                    touches = UnityEngine.Input.touches,
                });
            }
#else
            CheckMouseButton(0, leftMouseButtonFirstDown, leftMouseButtonDown, leftMouseButtonFirstUp);
            CheckMouseButton(1, rightMouseButtonFirstDown, rightMouseButtonDown, rightMouseButtonFirstUp);
            CheckMouseButton(2, middleMouseButtonFirstDown, middleMouseButtonDown, middleMouseButtonFirstUp);

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                firstHitGameObjectPosition = raycastCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                firstHitGameObject = Raycast(new Vector2(firstHitGameObjectPosition.x, firstHitGameObjectPosition.y));
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                Vector3 clickWorldPosition = raycastCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                GameObject hitGameObject = Raycast(new Vector2(clickWorldPosition.x, clickWorldPosition.y));

                if (firstHitGameObject != null && 
                    firstHitGameObject == hitGameObject && 
                    clickWorldPosition == firstHitGameObjectPosition)
                {
                    gameObjectLeftClicked.Raise(new GameObjectClickEventArgs()
                    {
                        gameObject = hitGameObject,
                        clickWorldPosition = clickWorldPosition
                    });

                    firstHitGameObject = null;
                }
            }

            float mouseScrollDelta = UnityEngine.Input.mouseScrollDelta.y;
            if (mouseScrollDelta != 0)
            {
                mouseScrolled.RaiseSilently(mouseScrollDelta);
            }
#endif
        }

        #endregion

        #region Utility Functions

        private void CheckMouseButton(
            int mouseButton, 
            Vector3Event mouseButtonFirstDown,
            Vector3Event mouseButtonDown,
            Vector3Event mouseButtonFirstUpEvent)
        {
            if (UnityEngine.Input.GetMouseButtonDown(mouseButton))
            {
                mouseButtonFirstDown.RaiseSilently(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetMouseButton(mouseButton))
            {
                mouseButtonDown.RaiseSilently(UnityEngine.Input.mousePosition);
            }

            if (UnityEngine.Input.GetMouseButtonUp(mouseButton))
            {
                mouseButtonFirstUpEvent.RaiseSilently(UnityEngine.Input.mousePosition);
            }
        }

        #endregion

        #region Raycasting

        private GameObject Raycast(Vector2 origin)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(origin, Vector2.zero);
            return raycastHit.transform != null ? raycastHit.transform.gameObject : null;
        }

        #endregion
    }
}
