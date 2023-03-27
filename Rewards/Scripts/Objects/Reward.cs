using Celeste.Objects;
using UnityEngine;

namespace Celeste.Rewards.Objects
{
    [CreateAssetMenu(fileName = nameof(Reward), menuName = "Celeste/Rewards/Reward")]
    public class Reward : ListScriptableObject<RewardItem>, IGuid
    {
        #region Properties and Fields

        public int Guid
        {
            get => guid;
            set => guid = value;
        }

        [SerializeField] private int guid;

        #endregion
        
        public void AwardReward()
        {
            foreach (RewardItem rewardItem in Items)
            {
                rewardItem.AwardReward();
            }
        }
    }
}
