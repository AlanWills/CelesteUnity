using Celeste.UI.Layout;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI.Layout
{
    [CustomEditor(typeof(GridLayoutContainer))]
    public class GridLayoutContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Layout"))
            {
                (target as GridLayoutContainer).Layout();
            }

            base.OnInspectorGUI();
        }
    }
}
