using Celeste.Objects;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Objects
{
    public class IEditorInitializableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawIInitializable(target as IEditorInitializable);

            base.OnInspectorGUI();
        } 
    
        public static void DrawIInitializable(IEditorInitializable initializable)
        {
            if (GUILayout.Button("Initialize"))
            {
                initializable.Editor_Initialize();
            }
        }      
    }
}