using System;
using System.Collections.Generic;
using Celeste.Tools;
using UnityEngine;

namespace Celeste.Debug.Menus
{
    [AddComponentMenu("Celeste/Debug/Menus/Debug Menus UI")]
    public class DebugMenusUI : MonoBehaviour
    {
        #region Properties and Fields

        private bool visible;
        private bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                inputBlocker.SetActive(visible);
                scrollPosition = Vector2.zero;
            }
        }

        [SerializeField] private Canvas debugAreaCanvas;
        [SerializeField] private GUISkin guiSkin;
        [SerializeField] private GameObject inputBlocker;
        [SerializeField] private RectTransform debugGuiDrawArea;
        [SerializeField] private float screenWidthDivisor = 600f;
        [SerializeField] private float screenHeightDivisor = 600f;

        [NonSerialized] private List<DebugMenu> debugMenus = new List<DebugMenu>();
        [NonSerialized] private Vector2 scrollPosition;
        [NonSerialized] private GUISkin oldSkin;

        #endregion

        #region Unity Methods

        private void OnValidate()
        {
            this.TryGetInParent(ref debugAreaCanvas);
        }
        
        private void Awake()
        {
            Visible = false;
            debugMenus.Clear();
        }

        private void OnGUI()
        {
            if (!Visible)
            {
                return;
            }

            if (oldSkin == null)
            {
                oldSkin = GUI.skin;
            }

            GUI.skin = guiSkin;

            Camera referenceCamera = debugAreaCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : debugAreaCanvas.worldCamera;
            Rect debugScreenSpaceRect = GetGuiSpaceRect(debugGuiDrawArea, referenceCamera);
            float xAspectRatio = debugScreenSpaceRect.width / screenWidthDivisor;
            float yAspectRatio = debugScreenSpaceRect.height / screenHeightDivisor;
            float maxAspectRatio = Mathf.Max(xAspectRatio, yAspectRatio);

            Vector3 scale = new Vector3(maxAspectRatio, maxAspectRatio, 1);
            GUI.matrix = Matrix4x4.Scale(scale);

            Rect screenRect = new Rect(debugScreenSpaceRect.xMin / maxAspectRatio, debugScreenSpaceRect.yMin / maxAspectRatio, debugScreenSpaceRect.width / maxAspectRatio, debugScreenSpaceRect.height / maxAspectRatio);
            Rect viewRect = new Rect(debugScreenSpaceRect.xMin, debugScreenSpaceRect.yMin, screenRect.width, screenRect.height * 4);

            // Take into account the vertical scroll bar here, so the debug menu doesn't go over it
            viewRect.size -= new Vector2(35, 0);

            using (GUI.ScrollViewScope scrollView = new GUI.ScrollViewScope(screenRect, scrollPosition, viewRect, false, true))
            {
                viewRect.position += new Vector2(10, 10);

                using (GUILayout.AreaScope areaScope = new GUILayout.AreaScope(viewRect))
                {
                    DebugMenu visibleDebugMenu = debugMenus.Find(x => x.Visible);

                    if (visibleDebugMenu != null)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            if (GUILayout.Button("Close"))
                            {
                                Visible = false;
                                visibleDebugMenu.Visible = false;
                            }

                            if (GUILayout.Button("Back"))
                            {
                                visibleDebugMenu.Visible = false;
                            }
                        }

                        visibleDebugMenu.DrawMenu();
                    }
                    else
                    {
                        if (GUILayout.Button("Close"))
                        {
                            Visible = false;
                        }

                        foreach (DebugMenu debugMenu in debugMenus)
                        {
                            if (GUILayout.Button(debugMenu.DisplayName, GUILayout.ExpandWidth(true)))
                            {
                                debugMenu.Visible = true;
                                scrollPosition = Vector2.zero;
                            }
                        }
                    }
                }

                scrollPosition = scrollView.scrollPosition;
            }

            GUI.skin = oldSkin;
        }

        private static Rect GetGuiSpaceRect(RectTransform transform, Camera referenceCamera)
        {
            Vector3[] worldCorners = new Vector3[4];
            transform.GetWorldCorners(worldCorners);

            Vector2 min = RectTransformUtility.WorldToScreenPoint(referenceCamera, worldCorners[0]);
            Vector2 max = RectTransformUtility.WorldToScreenPoint(referenceCamera, worldCorners[2]);

            float width = max.x - min.x;
            float height = max.y - min.y;

            // Flip Y for IMGUI because (0,0) is bottom-left in IMGUI, but top-left in ScreenPoint
            return new Rect(min.x, Screen.height - max.y, width, height);
        }

        #endregion

        #region Callbacks

        public void OnToggle()
        {
            Visible = !Visible;
        }

        public void RegisterDebugMenu(DebugMenu debugMenu)
        {
            if (debugMenu == null)
            {
                UnityEngine.Debug.LogAssertion($"Failed to register debug menu, as it was null.");
                return;
            }

            if (!debugMenus.Contains(debugMenu))
            {
                debugMenus.Add(debugMenu);
                debugMenus.Sort((a, b) =>
                { 
                    int priorityDiff = b.MenuPriority - a.MenuPriority;

                    if (priorityDiff != 0)
                    {
                        return priorityDiff;
                    }

                    return string.CompareOrdinal(a.DisplayName, b.DisplayName);
                });
            }
            else
            {
                UnityEngine.Debug.LogAssertion($"Failed to register debug menu.  {debugMenu.name} already exists in {nameof(DebugMenu)}.");
            }
        }

        public void DeregisterDebugMenu(DebugMenu debugMenu)
        {
            if (!debugMenus.Remove(debugMenu))
            {
                UnityEngine.Debug.LogAssertion($"Failed to deregister debug menu.  {debugMenu.name} does not exist in {nameof(DebugMenu)}.");
            }
        }

        #endregion
    }
}