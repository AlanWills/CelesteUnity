using Celeste.DeckBuilding;
using UnityEditor;
using UnityEngine;

namespace CelesteEditor.DeckBuilding
{
    [CustomEditor(typeof(CurrentHand))]
    public class CurrentHandEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CurrentHand currentHand = target as CurrentHand;

            for (int i = 0, n = currentHand.NumCards; i < n; ++i)
            {
                CardInstance cardInstance = currentHand.GetCard(i);

                GUILayout.Label($"{cardInstance.CardName} {cardInstance.CardGuid}");
            }

            base.OnInspectorGUI();
        }
    }
}
