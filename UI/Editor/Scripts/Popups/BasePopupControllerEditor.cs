using Celeste.UI;
using CelesteEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.UI
{
    [CustomEditor(typeof(BasePopupController), true, isFallback = true)]
    public class BasePopupControllerEditor : AdvancedEditor
    {
        protected override void DoOnEnable()
        {
            base.DoOnEnable();

            AddDrawPropertyCallback("popupElements", DrawPopupElements);
        }

        private void DrawPopupElements(SerializedProperty serializedProperty)
        {
            if (GUILayout.Button("Find Popup Elements"))
            {
                (target as BasePopupController).FindPopupElements_EditorOnly();
            }

            EditorGUILayout.PropertyField(serializedProperty);
        }
    }
}