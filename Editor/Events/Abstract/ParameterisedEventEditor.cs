using Celeste.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();

            argument = DrawArgument(argument);

            if (GUILayout.Button("Raise"))
            {
                (target as TEvent).Invoke(argument);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
