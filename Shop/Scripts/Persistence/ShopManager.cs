using Celeste.Persistence;
using Celeste.Shop.Catalogue;
using Celeste.Shop.Purchasing;
using UnityEngine;

namespace Celeste.Shop.Persistence
{
    [AddComponentMenu("Celeste/Shop/Shop Manager")]
    public class ShopManager : PersistentSceneManager<ShopManager, ShopDTO>
    {
        #region Properties and Fields

        protected override string FileName => FILE_NAME;

        public const string FILE_NAME = "Shop.dat";

        [SerializeField] private IAPCatalogue iapCatalogue;
        [SerializeField] private ShopPurchaser shopPurchaser;

        #endregion

        #region Save/Load

        protected override void Deserialize(ShopDTO dto)
        {
            InitializePurchaser();
        }

        protected override ShopDTO Serialize()
        {
            return new ShopDTO();
        }

        protected override void SetDefaultValues()
        {
            InitializePurchaser();
        }

        #endregion

        private void InitializePurchaser()
        {
            shopPurchaser.Initialize(iapCatalogue);
        }
    }
}
