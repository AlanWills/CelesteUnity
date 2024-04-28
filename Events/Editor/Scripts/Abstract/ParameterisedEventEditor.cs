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
            using (var horizontal = new GUILayout.HorizontalScope())
            {
                argument = DrawArgument(argument);

                if (GUILayout.Button("Invoke", GUILayout.ExpandWidth(false)))
                {
                    (target as TEvent).Invoke(argument);
                }
            }
            
            base.OnInspectorGUI();
        }

        #endregion
    }
}
