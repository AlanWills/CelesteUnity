using Celeste.Narrative.UI;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Narrative.UI
{
    [CustomEditor(typeof(CharacterView))]
    public class CharacterViewEditor : Editor
    {
        #region Properties and Fields

        private UIPosition uiPosition;

        #endregion

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            using (GUILayout.HorizontalScope horizontalScope = new GUILayout.HorizontalScope())
            {
                uiPosition = (UIPosition)EditorGUILayout.EnumPopup("UI Position", uiPosition);

                if (GUILayout.Button("Set UI Position", GUILayout.ExpandWidth(false)))
                {
                    (target as CharacterView).SetUIPosition(uiPosition);
                }
            }
        }
    }
}
