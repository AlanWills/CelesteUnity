using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(CompletionRewards), menuName = "Celeste/Live Ops/Rewards/Completion Rewards")]
    public class CompletionRewards : Celeste.Components.Component, ILiveOpCompletionRewards
    {
        #region Save Data

        [Serializable]
        public class CompletionRewardsData : ComponentData
        {
            public int[] rewardGuids;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new CompletionRewardsData();
        }

        public IReadOnlyList<int> GetCompletionRewardGuids(Instance instance)
        {
            CompletionRewardsData rewardsData = instance.data as CompletionRewardsData;
            return rewardsData.rewardGuids;
        }

        public IReadOnlyList<Reward> GetCompletionRewards(Instance instance, RewardCatalogue rewardCatalogue)
        {
            CompletionRewardsData rewardsData = instance.data as CompletionRewardsData;
            int[] rewardGuids = rewardsData.rewardGuids;
            List<Reward> rewards = new List<Reward>(rewardGuids.Length);

            for (int i = 0, n = rewardGuids.Length; i < n; ++i)
            {
                Reward reward = rewardCatalogue.MustFindByGuid(rewardGuids[i]);
                if (reward != null)
                {
                    rewards.Add(reward);
                }
            }

            return rewards;
        }

        public void AwardCompletionRewards(Instance instance, RewardCatalogue rewardCatalogue)
        {
            foreach (Reward reward in GetCompletionRewards(instance, rewardCatalogue))
            {
                reward.AwardReward();
            }
        }
    }
}