using Celeste.DeckBuilding.Cards;
using Celeste.DeckBuilding.Catalogue;
using CelesteEditor.DataStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DeckBuilding
{
    [CustomEditor(typeof(CardCatalogue))]
    public class CardCatalogueEditor : IIndexableItemsEditor<Card>
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Assign GUIDs"))
            {
                int highestUsedGuid = 0;
                List<SerializedObject> cardsToAssignGuid = new List<SerializedObject>();

                SerializedProperty itemsProperty = ItemsProperty;
                for (int i = 0, n = itemsProperty.arraySize; i < n; ++i)
                {
                    SerializedProperty serializedProperty = itemsProperty.GetArrayElementAtIndex(i);
                    SerializedObject card = new SerializedObject(serializedProperty.objectReferenceValue);
                    SerializedProperty guidProperty = card.FindProperty("guid");

                    if (guidProperty.intValue == 0)
                    {
                        cardsToAssignGuid.Add(card);
                    }
                    else
                    {
                        highestUsedGuid = Mathf.Max(highestUsedGuid, guidProperty.intValue);
                    }
                }

                for (int i = 0, n = cardsToAssignGuid.Count; i < n; ++i)
                {
                    SerializedObject card = cardsToAssignGuid[i];
                    SerializedProperty guidProperty = card.FindProperty("guid");
                    guidProperty.intValue = ++highestUsedGuid;
                    card.ApplyModifiedProperties();
                }

                AssetDatabase.SaveAssets();
            }

            base.OnInspectorGUI();
        }
    }
}
