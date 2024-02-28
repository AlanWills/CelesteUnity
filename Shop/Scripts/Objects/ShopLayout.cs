using System;
using System.Collections.Generic;
using Celeste.Shop.Catalogue;
using UnityEngine;

namespace Celeste.Shop
{
    [Serializable]
    public struct ShopSectionLayout
    {
        public string sectionName;
        public GameObject shopSectionPrefab;
        public List<ShopItemLayout> shopItemLayouts;
    }

    [Serializable]
    public struct ShopItemLayout
    {
        public ShopItem shopItem;
        public GameObject shopItemPrefab;
    }
    
    [CreateAssetMenu(fileName = nameof(ShopLayout), order = CelesteMenuItemConstants.SHOP_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.SHOP_MENU_ITEM + "Shop Layout")]
    public class ShopLayout : ScriptableObject
    {
        #region Properties and Fields

        public IReadOnlyList<ShopSectionLayout> ShopSectionLayouts => shopSectionLayouts;

        [SerializeField] private List<ShopSectionLayout> shopSectionLayouts = new();

        #endregion
    }
}