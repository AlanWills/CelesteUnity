using System;
using UnityEngine;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop UI Controller")]
    public class ShopUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ShopLayout shopLayout;
        [SerializeField] private RectTransform shopSectionsParent;
        
        #endregion
        
        #region Unity Methods

        private void OnEnable()
        {
            RefreshUI();
        }

        #endregion
        
        private void RefreshUI()
        {
            var shopSections = shopLayout.ShopSectionLayouts;

            for (int i = 0, n = shopSections.Count; i < n; ++i)
            {
                SetupSectionUI(shopSections[i]);
            }
        }

        private void SetupSectionUI(ShopSectionLayout shopSectionLayout)
        {
            GameObject shopSectionUI = Instantiate(shopSectionLayout.shopSectionPrefab, shopSectionsParent);
            IShopSectionUIController shopSectionUIController = shopSectionUI.GetComponent<IShopSectionUIController>();
            Debug.Assert(shopSectionUIController != null, $@"No script implementing the {nameof(IShopSectionUIController)} interface could be found on {shopSectionUI.name}.");
            shopSectionUIController.Hookup(shopSectionLayout);
            
            for (int i = 0, n = shopSectionLayout.shopItemLayouts.Count; i < n; ++i)
            {
                SetupItemUI(shopSectionUIController, shopSectionLayout.shopItemLayouts[i]);
            }
        }

        private void SetupItemUI(IShopSectionUIController shopSectionUIController, ShopItemLayout shopItemLayout)
        {
            GameObject shopItemUI = Instantiate(shopItemLayout.shopItemPrefab, shopSectionUIController.ShopItemParent);
            IShopItemUIController shopItemUIController = shopItemUI.GetComponent<IShopItemUIController>();
            Debug.Assert(shopItemUIController != null, $@"No script implementing the {nameof(IShopItemUIController)} interface could be found on {shopItemUI.name}.");
            
            shopItemUIController.Hookup(shopItemLayout);
        }
    }
}