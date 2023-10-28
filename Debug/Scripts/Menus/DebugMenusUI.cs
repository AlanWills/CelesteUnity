using System;
using System.Collections.Generic;
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
            get { return visible; }
            set
            {
                visible = value;
                inputBlocker.SetActive(visible);
            }
        }

        [SerializeField] private GUISkin guiSkin;
        [SerializeField] private GameObject inputBlocker;
        [SerializeField] private RectTransform debugGuiDrawArea;
        [SerializeField] private float screenWidthDivisor = 600f;
        [SerializeField] private float screenHeightDivisor = 600f;
        [SerializeField] private float screenOffset = 0;

        [NonSerialized] private List<DebugMenu> debugMenus = new List<DebugMenu>();
        [NonSerialized] private Vector2 scrollPosition;
        [NonSerialized] private GUISkin oldSkin;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Visible = false;
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

            Rect debugScreenSpaceRect = GetGuiSpaceRect(debugGuiDrawArea);
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
                        if (GUILayout.Button("Back"))
                        {
                            visibleDebugMenu.Visible = false;
                        }

                        visibleDebugMenu.DrawMenu();
                    }
                    else
                    {
                        foreach (DebugMenu debugMenu in debugMenus)
                        {
                            if (GUILayout.Button(debugMenu.DisplayName, GUILayout.ExpandWidth(true)))
                            {
                                debugMenu.Visible = true;
                            }
                        }
                    }
                }

                scrollPosition = scrollView.scrollPosition;
            }

            GUI.skin = oldSkin;
        }

        private static Rect GetGuiSpaceRect(RectTransform transform)
        {
            Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
            Rect rect = new Rect((Vector2)transform.position - (size * transform.pivot), size);
#if UNITY_ANDROID
            rect.y += (Screen.height - rect.height);    // Adjust for safe area, but only on Android.  Don't know why, but it works...
#endif

            return rect;
        }

#endregion

#region Callbacks

        public void OnToggle()
        {
            Visible = !Visible;
        }

        public void RegisterDebugMenu(DebugMenu debugMenu)
        {
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