using Celeste.Objects;
using Celeste.Parameters;
using Celeste.Wallet;
using UnityEngine;

namespace Celeste.Rewards.Catalogue
{
    [CreateAssetMenu(fileName = nameof(Reward), menuName = "Celeste/Rewards/Reward")]
    public class Reward : ScriptableObject, IGuid, IInitializable
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        public Sprite Icon => currency.Icon;
        public int Quantity => quantity.Value;

        [SerializeField] private int guid;
        [SerializeField] private Currency currency;
        [SerializeField] private IntReference quantity;

        #endregion

        public void Initialize()
        {
            if (quantity == null)
            {
                quantity = CreateInstance<IntReference>();
                quantity.name = "RewardQuantity";
                quantity.hideFlags = HideFlags.HideInHierarchy;
#if UNITY_EDITOR
                UnityEditor.AssetDatabase.AddObjectToAsset(quantity, this);
#endif
            }
        }

        public void AwardReward()
        {
            currency.Quantity += Quantity;
        }

        #region Factory Functions

        public static Reward FromCurrency(Currency currency)
        {
            Reward reward = CreateInstance<Reward>();
            reward.Initialize();
            reward.name = currency.name;
            reward.guid = -1;
            reward.currency = currency;
            reward.quantity.IsConstant = true;
            reward.quantity.Value = currency.Quantity;

            return reward;
        }

        #endregion
    }
}
