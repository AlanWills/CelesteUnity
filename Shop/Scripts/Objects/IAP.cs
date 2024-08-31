using Celeste.Objects;
using UnityEngine;
using Celeste.Tools;
using Celeste.Tools.Attributes.GUI;

namespace Celeste.Shop.Objects
{
    [CreateAssetMenu(fileName = nameof(IAP), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "IAP")]
    public class IAP : ScriptableObject, IIntGuid
    {
        public enum Type
        {
            Consumable,
            NonConsumable,
            Subscription
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

        public Type ProductType => productType;
        public string IAPCode => iapCode;
        public string GoogleIAPCode => hasGoogleSpecificCode ? googleSpecificCode : iapCode;
        public string AppleIAPCode => hasAppleSpecificCode ? appleSpecificCode : iapCode;

        public string LocalisedPriceString { get; set; } = "???";
        public string LocalisedTitle { get; set; }
        public string LocalisedDescription { get; set; }
        public string ISOCurrencyCode { get; set; } = "USD";
        public float LocalisedPrice { get; set; }

        [SerializeField] private int guid;
        [SerializeField] private Type productType = Type.Consumable;
        [SerializeField] private string iapCode;
        [SerializeField] private bool hasGoogleSpecificCode = false;
        [SerializeField, ShowIf(nameof(hasGoogleSpecificCode))] private string googleSpecificCode;
        [SerializeField] private bool hasAppleSpecificCode = false;
        [SerializeField, ShowIf(nameof(hasAppleSpecificCode))] private string appleSpecificCode;

        #endregion
    }
}
