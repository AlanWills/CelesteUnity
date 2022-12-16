using Celeste.BoardGame.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.BoardGame.UI
{
    [CustomEditor(typeof(Die2DBoardGameObjectComponentUIController))]
    public class Die2DBoardGameObjectComponentUIControllerEditor : Editor
    {
        private int desiredValue;

        public override void OnInspectorGUI()
        {
            using (var horizontal = new EditorGUILayout.HorizontalScope())
            {
                desiredValue = EditorGUILayout.IntField(desiredValue);

                if (GUILayout.Button("Set Value", GUILayout.ExpandWidth(false)))
                {
                    (target as Die2DBoardGameObjectComponentUIController).SetValue(desiredValue);
                }
            }

            base.OnInspectorGUI();
        }
    }
}
