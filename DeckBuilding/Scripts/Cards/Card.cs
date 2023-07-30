using Celeste.Components;
using Celeste.DeckBuilding.Components;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [CreateAssetMenu(fileName = nameof(Card), menuName = "Celeste/Deck Building/Cards/Card")]
    public class Card : ComponentContainerUsingSubAssets<CardComponent>, IIntGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Sprite CardBack
        {
            get { return cardBack; }
            set
            {
                cardBack = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        public Sprite CardFront
        {
            get { return cardFront; }
            set
            {
                cardFront = value;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        [SerializeField] private int guid;
        [SerializeField] private Sprite cardBack;
        [SerializeField] private Sprite cardFront;

        #endregion
    }
}
