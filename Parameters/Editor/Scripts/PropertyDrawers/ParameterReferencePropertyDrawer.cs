using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.PropertyDrawers.Parameters
{
    public abstract class ParameterReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect initialPosition = position;
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (property.objectReferenceValue != null)
            {
                SerializedObject serializedReference = new SerializedObject(property.objectReferenceValue);
                serializedReference.Update();

                SerializedProperty isConstantProperty = serializedReference.FindProperty("isConstant");
                if (isConstantProperty != null)
                {
                    Rect constantToggleRect = new Rect(position.x, position.y, 16, initialPosition.height);
                    using (EditorGUI.ChangeCheckScope changeScope = new EditorGUI.ChangeCheckScope())
                    {
                        isConstantProperty.boolValue = EditorGUI.Toggle(constantToggleRect, isConstantProperty.boolValue, EditorStyles.radioButton);

                        if (isConstantProperty.boolValue)
                        {
                            serializedReference.FindProperty("referenceValue").objectReferenceValue = null;
                        }
                    }

                    Rect valueRect = new Rect(constantToggleRect.x + 20, constantToggleRect.y, initialPosition.width - constantToggleRect.x, initialPosition.height);
                    if (isConstantProperty.boolValue)
                    {
                        EditorGUI.PropertyField(valueRect, serializedReference.FindProperty("constantValue"), GUIContent.none);
                    }
                    else
                    {
                        EditorGUI.PropertyField(valueRect, serializedReference.FindProperty("referenceValue"), GUIContent.none);
                    }
                }

                serializedReference.ApplyModifiedProperties();
            }
            else
            {
                EditorGUI.PropertyField(position, property);
            }

            EditorGUI.EndProperty();
        }
    }
}
