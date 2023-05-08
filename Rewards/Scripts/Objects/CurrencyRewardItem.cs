using Celeste.Wallet;
using UnityEngine;

namespace Celeste.Rewards.Objects
{
    [CreateAssetMenu(fileName = nameof(CurrencyRewardItem), menuName = "Celeste/Rewards/Currency Reward Item")]
    public class CurrencyRewardItem : RewardItem
    {
        #region Properties and Fields

        public override Sprite Icon => currency.Icon;

        [SerializeField] private Currency currency;

        #endregion

        public override void AwardReward(int rewardMulplier)
        {
            rewardMulplier = CanBeMultiplied ? rewardMulplier : 1;
            currency.Quantity += (Quantity * rewardMulplier);
        }

        #region Factory Functions

        public static CurrencyRewardItem FromCurrency(Currency currency)
        {
            CurrencyRewardItem currencyRewardItem = CreateInstance<CurrencyRewardItem>();
            currencyRewardItem.Initialize();
            currencyRewardItem.name = currency.name;
            currencyRewardItem.Guid = -1;
            currencyRewardItem.currency = currency;
            currencyRewardItem.quantity.IsConstant = true;
            currencyRewardItem.quantity.Value = currency.Quantity;

            return currencyRewardItem;
        }

        #endregion
    }
}
