using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Objects
{
    public class IInitializableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawIInitializable(target as IInitializable);

            base.OnInspectorGUI();
        } 
    
        public static void DrawIInitializable(IInitializable initializable)
        {
            if (GUILayout.Button("Initialize"))
            {
                initializable.Initialize();
            }
        }      
    }
}