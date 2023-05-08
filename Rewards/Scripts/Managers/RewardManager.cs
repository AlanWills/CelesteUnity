using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Rewards.Managers
{
    [AddComponentMenu("Celeste/Rewards/Managers/Reward Manager")]
    public class RewardManager : MonoBehaviour
    {
        #region Callbacks

        public void OnRequestAwardReward(Reward reward)
        {
            reward.AwardReward(1);
        }

        #endregion
    }
}
