using Celeste.Localisation.UI;
using Celeste.Shop.Catalogue;
using UnityEngine;

namespace Celeste.Shop.UI
{
    [AddComponentMenu("Celeste/Shop/UI/Shop Item Cost UI Controller")]
    public class ShopItemCostUIController : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private ParameterisedTextUI costQuantityText;

        #endregion

        public void Hookup(ShopItem shopItem)
        {
            costQuantityText.SetUp(
                "GLYPH", $"\"{shopItem.Cost.currency.GlyphName}\"",
                "COST", shopItem.Cost.quantity.ToString());
        }
    }
}
