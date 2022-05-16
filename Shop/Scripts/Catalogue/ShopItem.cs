using Celeste.Objects;
using Celeste.Rewards.Catalogue;
using Celeste.Shop.Purchasing;
using Celeste.Tools.Attributes.GUI;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Shop.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ShopItem), menuName = "Celeste/Shop/Shop Item")]
    public class ShopItem : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid 
        { 
            get => guid; 
            set => guid = value; 
        }

        public Cost Cost => cost;
        public IReadOnlyList<Reward> Rewards => rewards;

        [SerializeField] private int guid;
        [SerializeField, InlineDataInInspector] private Cost cost;
        [SerializeField] private List<Reward> rewards = new List<Reward>();

        #endregion
    }
}
