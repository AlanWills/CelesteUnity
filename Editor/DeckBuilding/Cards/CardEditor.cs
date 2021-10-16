using Celeste.DeckBuilding.Cards;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DeckBuilding.Cards
{
    [CustomEditor(typeof(Card))]
    public class CardEditor : Editor
    {
        #region Properties and Fields

        private Card Card
        {
            get { return target as Card; }
        }

        private SerializedProperty componentsProperty;
        private int selectedTypeIndex = 0;

        #endregion

        private void OnEnable()
        {
            componentsProperty = serializedObject.FindProperty("components");
        }

        #region GUI

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script", "components");

            selectedTypeIndex = CelesteEditorGUILayout.SubAssetListField(
                "Components",
                CardEditorConstants.AllCardComponentTypes,
                CardEditorConstants.AllCardComponentDisplayNames,
                selectedTypeIndex,
                componentsProperty,
                Card.AddComponent,
                Card.RemoveComponent);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}