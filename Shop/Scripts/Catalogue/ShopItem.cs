using Celeste.Objects;
using Celeste.Rewards.Catalogue;
using Celeste.Shop.Purchasing;
using Celeste.Tools.Attributes.GUI;
using System.Collections.Generic;
using Celeste.Localisation;
using Celeste.Rewards.Objects;
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

        public LocalisationKey Title => title;
        public Sprite Icon => icon;
        public Cost Cost => cost;
        public Reward Reward => reward;

        [SerializeField] private int guid;
        [SerializeField] private LocalisationKey title;
        [SerializeField] private Sprite icon;
        [SerializeField, InlineDataInInspector] private Cost cost;
        [SerializeField] private Reward reward;

        #endregion
    }
}
