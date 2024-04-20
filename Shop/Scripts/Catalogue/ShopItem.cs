using Celeste.Objects;
using Celeste.Shop.Purchasing;
using Celeste.Tools.Attributes.GUI;
using Celeste.Localisation;
using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Shop.Catalogue
{
    [CreateAssetMenu(fileName = nameof(ShopItem), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Item")]
    public class ShopItem : ScriptableObject, IIntGuid
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
