using System;
using Celeste.Memory;
using Celeste.Rewards.Objects;
using UnityEngine;

namespace Celeste.Rewards.UI
{
    [AddComponentMenu("Celeste/Rewards/UI/Reward UI")]
    public class RewardUI : MonoBehaviour
    {
        #region Properties and Fields

        [SerializeField] private GameObjectAllocator rewardItemUIAllocator;

        #endregion

        public void Hookup(Reward reward)
        {
            Debug.Assert(reward != null, $"Null reward inputted into {nameof(RewardUI)}.");
            if (reward != null)
            {
                for (int i = 0, n = reward.NumItems; i < n; ++i)
                {
                    var rewardItemInfo = reward.GetItem(i);

                    if (rewardItemInfo.ShouldAward)
                    {
                        GameObject rewardItemUIGameObject = rewardItemUIAllocator.Allocate();
                        Debug.Assert(rewardItemUIGameObject != null,
                            $"Failed to allocate UI for item in reward {reward.Items}.  Exceeded max allocator capacity of {rewardItemUIAllocator.Capacity}.");

                        if (rewardItemUIGameObject != null)
                        {
                            RewardItemUI rewardItemUI = rewardItemUIGameObject.GetComponent<RewardItemUI>();
                            rewardItemUI.Hookup(rewardItemInfo.rewardItem);
                            rewardItemUIGameObject.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
        
        #region Unity Methods

        private void OnDisable()
        {
            rewardItemUIAllocator.DeallocateAll();
        }

        #endregion
    }
}
