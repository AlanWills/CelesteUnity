namespace Celeste.Shop
{
    public interface IShopItemUIController
    {
        void Hookup(ShopItemLayout shopItemLayout);
        void RefreshPrice();
    }
}