using Celeste.Objects;
using Celeste.Wallet;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(Reward), menuName = "Celeste/Rewards/Reward")]
    public class Reward : ScriptableObject, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        public Sprite Icon => currency.Icon;
        public int Quantity => quantity;

        [SerializeField] private int guid;
        [SerializeField] private Currency currency;
        [SerializeField] private int quantity;

        #endregion

        public void AwardReward()
        {
            currency.Quantity += quantity;
        }

        #region Factory Functions

        public static Reward FromCurrency(Currency currency)
        {
            Reward reward = CreateInstance<Reward>();
            reward.name = currency.name;
            reward.guid = -1;
            reward.currency = currency;
            reward.quantity = currency.Quantity;

            return reward;
        }

        #endregion
    }
}
