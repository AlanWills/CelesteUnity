using Celeste.Tilemaps;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tilemaps.Viewport
{
    [CustomEditor(typeof(TilemapZoom))]
    public class TilemapZoomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Fit Camera"))
            {
                (target as TilemapZoom).FitCamera();
            }
            
            base.OnInspectorGUI();
        }
    }
}