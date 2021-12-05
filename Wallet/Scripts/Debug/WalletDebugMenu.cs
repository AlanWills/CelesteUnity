using Celeste.Debug.Menus;
using System.Collections;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Wallet.Debug
{
    [CreateAssetMenu(fileName = nameof(WalletDebugMenu), menuName = "Celeste/Wallet/Debug/Wallet Debug Menu")]
    public class WalletDebugMenu : DebugMenu
    {
        #region Properties and Fields

        [SerializeField] private WalletRecord walletRecord;

        private int quantityChange = 100;

        #endregion

        protected override void OnDrawMenu()
        {
            string quantityChangeText = quantityChange.ToString();
            quantityChangeText = TextField(quantityChangeText);

            if (GUI.changed)
            {
                int.TryParse(quantityChangeText, out quantityChange);
            }

            for (int i = 0, n = walletRecord.NumCurrencyRecords; i < n; ++i)
            {
                Currency currency = walletRecord.GetCurrency(i);
                int quantity = walletRecord.GetQuantity(i);

                using (var horizontal = new HorizontalScope())
                {
                    Label($"{currency.name}: {quantity}");

                    if (Button("Add", ExpandWidth(false)))
                    {
                        walletRecord.AddCurrency(currency, quantityChange);
                    }

                    if (Button("Remove", ExpandWidth(false)))
                    {
                        walletRecord.RemoveCurrency(currency, quantityChange);
                    }
                }
            }
        }
    }
}