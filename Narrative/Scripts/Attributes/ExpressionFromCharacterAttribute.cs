using Celeste.DataStructures;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;
using UnityEngine;
#if UNITY_EDITOR
using Celeste.Narrative.Characters;
using Celeste.Narrative.Characters.Components;
using UnityEditor;
#endif

namespace Celeste.Narrative.Attributes
{
    public class ExpressionFromCharacterAttribute : MultiPropertyAttribute, IGUIAttribute, IGetHeightAttribute
    {
        private string CharacterProperyName { get; }

        public ExpressionFromCharacterAttribute(string characterProperyName)
        {
            CharacterProperyName = characterProperyName;
        }
        
        #if UNITY_EDITOR
        
        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!TryGetCharacter2DArtComponent(property, out Character2DArtComponent character2DArtComponent) || 
                character2DArtComponent.NumExpressionNames == 0)
            {
                return 0;
            }

            return GetDefaultPropertyHeight(property, label);
        }
        
        public Rect OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!TryGetCharacter2DArtComponent(property, out Character2DArtComponent character2DArtComponent))
            {
                return position;
            }

            int currentSelectedExpression = character2DArtComponent.ExpressionNames.FindIndex(x => string.CompareOrdinal(x, property.stringValue) == 0);
            if (currentSelectedExpression < 0)
            {
                currentSelectedExpression = Character2DArtComponent.DefaultExpressionNameIndex;
            }
            
            int newSelectedExpression = EditorGUI.Popup(position, label.text, currentSelectedExpression, character2DArtComponent.ExpressionNames);

            if (currentSelectedExpression != newSelectedExpression)
            {
                property.stringValue = newSelectedExpression == Character2DArtComponent.DefaultExpressionNameIndex ? 
                    string.Empty : character2DArtComponent.ExpressionNames[newSelectedExpression];
            }

            return position;
        }

        private bool TryGetCharacter2DArtComponent(SerializedProperty property, out Character2DArtComponent  character2DArtComponent)
        {
            SerializedProperty characterProperty = property.serializedObject.FindProperty(CharacterProperyName);
            Character character = characterProperty.objectReferenceValue as Character;
            character2DArtComponent = character.FindComponent<Character2DArtComponent>();
            return character2DArtComponent != null;
        }

        #endif
    }
}