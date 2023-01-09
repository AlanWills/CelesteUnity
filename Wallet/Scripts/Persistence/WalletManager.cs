using Celeste.Persistence;
using Celeste.Wallet.Persistence;
using UnityEngine;

namespace Celeste.Wallet
{
    [AddComponentMenu("Celeste/Wallet/Wallet Manager")]
    public class WalletManager : PersistentSceneManager<WalletManager, WalletDTO>
    {
        #region Properties and Fields

        public static readonly string FILE_NAME = "Wallet.dat";
        protected override string FileName
        {
            get { return FILE_NAME; }
        }

        [SerializeField] private CurrencyCatalogue currencyCatalogue;
        [SerializeField] private WalletRecord walletRecord;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            walletRecord.Initialize(currencyCatalogue);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            walletRecord.Shutdown();
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(WalletDTO dto)
        {
            foreach (var currencyDto in dto.currencies)
            {
                Currency currency = currencyCatalogue.FindByGuid(currencyDto.currencyGuid);
                UnityEngine.Debug.Assert(currency != null, $"Could not find currency with guid {currencyDto.currencyGuid} in catalogue.");

                if (currency != null)
                {
                    currency.Quantity = currencyDto.quantity;
                }
            }
        }

        protected override WalletDTO Serialize()
        {
            return new WalletDTO(walletRecord);
        }

        protected override void SetDefaultValues()
        {
            walletRecord.CreateStartingWallet();
        }

        #endregion
    }
}
