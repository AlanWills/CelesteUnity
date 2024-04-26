using Celeste.Scene;
using Celeste.Scene.Debug;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Scene.Debug
{
    [CustomEditor(typeof(SceneSetDebugMenu))]
    public class SceneSetDebugMenuEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Find All"))
            {
                AssetDatabaseExtensions.FindAssets<SceneSet>(target, "sceneSets");
            }

            base.OnInspectorGUI();
        }
    }
}