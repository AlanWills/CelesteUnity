using Celeste.Persistence;
using Celeste.Wallet.Persistence;
using System.Collections.Generic;
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

        protected override void OnDestroy()
        {
            base.OnDestroy();

            walletRecord.Shutdown();
        }

        #endregion

        #region Save/Load Methods

        protected override void Deserialize(WalletDTO dto)
        {
            Dictionary<int, int> currencyAmountLookup = new Dictionary<int, int>();

            foreach (var currencyDto in dto.currencies)
            {
                currencyAmountLookup[currencyDto.currencyGuid] = currencyDto.quantity;
            }
            
            walletRecord.Initialize(currencyCatalogue, currencyAmountLookup);
        }

        protected override WalletDTO Serialize()
        {
            return new WalletDTO(walletRecord);
        }

        protected override void SetDefaultValues()
        {
            walletRecord.Initialize(currencyCatalogue);
        }

        #endregion
    }
}
