using Celeste.Tilemaps;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Tilemaps.Viewport
{
    [CustomEditor(typeof(TilemapDrag))]
    public class TilemapDragEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Centre Camera"))
            {
                (target as TilemapDrag).CentreCamera();
            }
            
            base.OnInspectorGUI();
        }
    }
}