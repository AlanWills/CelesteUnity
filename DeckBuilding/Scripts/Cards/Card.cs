using Celeste.Components;
using Celeste.Objects;
using UnityEngine;

namespace Celeste.DeckBuilding.Cards
{
    [CreateAssetMenu(fileName = nameof(Card), menuName = "Celeste/Deck Building/Cards/Card")]
    public class Card : ComponentContainerUsingSubAssets<Components.Component>, IGuid
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

        [SerializeField] private int guid;

        #endregion
    }
}
