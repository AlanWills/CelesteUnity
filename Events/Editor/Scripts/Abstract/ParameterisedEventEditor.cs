using Celeste.Events;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.Events
{
    public abstract class ParameterisedEventEditor<T, TEvent> : Editor where TEvent : ParameterisedEvent<T>
    {
        #region Properties and Fields

        private T argument;

        #endregion

        #region GUI

        protected abstract T DrawArgument(T argument);

        public override void OnInspectorGUI()
        {
            using (new GUILayout.HorizontalScope())
            {
                argument = DrawArgument(argument);

                if (GUILayout.Button("Invoke", GUILayout.ExpandWidth(false)))
                {
                    (target as TEvent).Invoke(argument);
                }
            }

            EditorGUILayout.PrefixLabel("Help Text:");
            SerializedProperty helpTextProperty = serializedObject.FindProperty("helpText");
            helpTextProperty.stringValue = EditorGUILayout.TextArea(helpTextProperty.stringValue);
        }

        #endregion
    }
}
