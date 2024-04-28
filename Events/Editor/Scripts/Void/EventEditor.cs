using Celeste.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace CelesteEditor.Events
{
    [CustomEditor(typeof(Event))]
    public class EventEditor : Editor
    {
        #region GUI

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (UnityEngine.GUILayout.Button("Invoke"))
            {
                (target as Event).Invoke();
            }
        }

        #endregion
    }
}