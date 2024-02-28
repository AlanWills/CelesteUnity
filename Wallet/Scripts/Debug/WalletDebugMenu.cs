using Celeste.Debug.Menus;
using System.Collections;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Celeste.Wallet.Debug
{
    [CreateAssetMenu(fileName = nameof(WalletDebugMenu), order = CelesteMenuItemConstants.WALLET_MENU_ITEM_PRIORITY, menuName = CelesteMenuItemConstants.WALLET_MENU_ITEM + "Debug/Wallet Debug Menu")]
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

            for (int i = 0, n = walletRecord.NumCurrencies; i < n; ++i)
            {
                Currency currency = walletRecord.GetCurrency(i);

                using (var horizontal = new HorizontalScope())
                {
                    Label($"{currency.name}: {currency.Quantity}");

                    if (Button("Add", ExpandWidth(false)))
                    {
                        currency.Quantity += quantityChange;
                    }

                    if (Button("Remove", ExpandWidth(false)))
                    {
                        currency.Quantity -= quantityChange;
                    }

                    if (Button("Set To Zero", ExpandWidth(false)))
                    {
                        currency.Quantity = 0;
                    }
                }
            }
        }
    }
}