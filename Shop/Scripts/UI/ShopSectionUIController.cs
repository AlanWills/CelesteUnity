using TMPro;
using UnityEngine;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop Section UI Controller")]
    public class ShopSectionUIController : MonoBehaviour, IShopSectionUIController
    {
        #region Properties and Fields

        public Transform ShopItemParent => shopItemParent;

        [SerializeField] private Transform shopItemParent;
        [SerializeField] private TextMeshProUGUI sectionTitleText;
        
        #endregion
        
        public void Hookup(ShopSectionLayout shopSectionLayout)
        {
            sectionTitleText.text = shopSectionLayout.sectionName;
        }
    }
}