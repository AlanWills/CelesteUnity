using CelesteEditor.Tools.PropertyDrawers.Attributes;
using UnityEngine;
using UnityEditor;

namespace CelesteEditor.BuildSystem.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(AppVersion))]
    public class AppVersionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue != null)
            {
                GUIContent versionContent = new GUIContent(property.objectReferenceValue.ToString());
                float widthOfVersion = GUI.skin.label.CalcSize(versionContent).x + EditorGUIUtility.labelWidth;
                Rect versionStringRect = new Rect(position.x, position.y, widthOfVersion, position.height);
                EditorGUI.LabelField(versionStringRect, label, versionContent);

                Rect propertyRect = new Rect(versionStringRect.x + widthOfVersion + 5, position.y, position.width - widthOfVersion, position.height);
                EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}