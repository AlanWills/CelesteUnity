using UnityEngine;

namespace Celeste.Shop
{
    public interface IShopSectionUIController
    {
        Transform ShopItemParent { get; }
        
        void Hookup(ShopSectionLayout shopSectionLayout);
    }
}