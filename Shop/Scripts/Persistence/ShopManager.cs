using Celeste.Persistence;
using UnityEngine;

namespace Celeste.Shop.Persistence
{
    [AddComponentMenu("Celeste/Shop/Shop Manager")]
    public class ShopManager : PersistentSceneManager<ShopManager, ShopDTO>
    {
        #region Properties and Fields

        protected override string FileName => FILE_NAME;

        public const string FILE_NAME = "Shop.dat";

        #endregion

        #region Save/Load

        protected override void Deserialize(ShopDTO dto)
        {
        }

        protected override ShopDTO Serialize()
        {
            return new ShopDTO();
        }

        protected override void SetDefaultValues()
        {
        }

        #endregion
    }
}
