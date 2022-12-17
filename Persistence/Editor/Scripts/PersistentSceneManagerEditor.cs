using Celeste.Persistence;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Persistence
{
    public class IPersistentSceneManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Load"))
                {
                    (target as IPersistentSceneManager).Load();
                }

                if (GUILayout.Button("Save"))
                {
                    (target as IPersistentSceneManager).Save();
                }

                if (GUILayout.Button("Serialize"))
                {
                    GUIUtility.systemCopyBuffer = (target as IPersistentSceneManager).SerializeToString();
                }
            }

            base.OnInspectorGUI();
        }
    }
}
