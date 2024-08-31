using Celeste.Objects;
using Celeste.Shop.Purchasing;
using Celeste.Tools.Attributes.GUI;
using Celeste.Localisation;
using Celeste.Rewards.Objects;
using UnityEngine;
using Celeste.Tools;
using Celeste.Shop.Objects;

namespace Celeste.Shop
{
    [CreateAssetMenu(fileName = nameof(ShopItem), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Item")]
    public class ShopItem : ScriptableObject, IIntGuid
    {
        private enum CostType
        {
            Currency,
            IAP
        }

        #region Properties and Fields

        public int Guid 
        { 
            get => guid; 
            set
            {
                if (guid != value)
                {
                    guid = value;
                    EditorOnly.SetDirty(this);
                }
            }
        }

        public LocalisationKey Title => title;
        public Sprite Icon => icon;
        public bool IsCurrencyPurchase => purchaseType == CostType.Currency;
        public bool IsIAPPurchase => purchaseType == CostType.IAP;
        public Cost Cost => cost;
        public IAP IAP => iap;
        public Reward Reward => reward;

        [SerializeField] private int guid;
        [SerializeField] private LocalisationKey title;
        [SerializeField] private Sprite icon;
        [SerializeField] private CostType purchaseType = CostType.Currency;
        [SerializeField, ShowIfEnum(nameof(purchaseType), (int)CostType.Currency), InlineDataInInspector] private Cost cost;
        [SerializeField, ShowIfEnum(nameof(purchaseType), (int)CostType.IAP)] private IAP iap;
        [SerializeField] private Reward reward;

        #endregion
    }
}
