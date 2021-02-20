using Celeste.FSM.Nodes.Events;
using Celeste.FSM.Nodes.Events.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.FSM.Nodes.Events.Conditions
{
    public class EventConditionEditor
    {
        #region Properties and Fields
        
        public static readonly EventConditionEditor DefaultEventConditionEditor = new EventConditionEditor();

        #endregion

        public void GUI(MultiEventNode eventNode, EventCondition eventCondition)
        {
            SerializedObject serializedObject = new SerializedObject(eventCondition);
            serializedObject.Update();

            OnGUI(eventNode, serializedObject);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnGUI(MultiEventNode eventNode, SerializedObject eventConditionObject)
        {
            SerializedProperty nameProperty = eventConditionObject.FindProperty("m_Name");
            SerializedProperty listenForProperty = eventConditionObject.FindProperty("listenFor");
            UnityEngine.Object oldValue = listenForProperty.objectReferenceValue;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(listenForProperty, GUIContent.none);

            if (EditorGUI.EndChangeCheck())
            {
                UnityEngine.Object newValue = listenForProperty.objectReferenceValue;

                if (oldValue == null && newValue != null)
                {
                    nameProperty.stringValue = newValue.name;
                    eventNode.AddEventConditionPort(newValue.name);
                }
                else if (oldValue != null && newValue != null && oldValue != newValue)
                {
                    nameProperty.stringValue = newValue.name;
                    eventNode.RemoveDynamicPort(oldValue.name);
                    eventNode.AddEventConditionPort(newValue.name);
                }
                else if (oldValue != null && newValue == null)
                {
                    nameProperty.stringValue = "";
                    eventNode.RemoveDynamicPort(oldValue.name);
                }
            }
        }
    }
}
