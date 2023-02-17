using Celeste.Rewards.Catalogue;
using UnityEngine;

namespace Celeste.Rewards.Managers
{
    [AddComponentMenu("Celeste/Rewards/Managers/Reward Manager")]
    public class RewardManager : MonoBehaviour
    {
        #region Callbacks

        public void OnRequestAwardReward(Reward reward)
        {
            reward.AwardReward();
        }

        #endregion
    }
}
