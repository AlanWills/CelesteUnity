using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System;
using System.Collections.Generic;
using Celeste.Rewards.Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(CompletionReward), menuName = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM + "Rewards/Completion Reward", order = CelesteMenuItemConstants.LIVEOPS_MENU_ITEM_PRIORITY)]
    public class CompletionReward : Celeste.Components.BaseComponent, ILiveOpCompletionReward
    {
        #region Save Data

        [Serializable]
        public class CompletionRewardData : ComponentData
        {
            public int RewardGuid;
            public bool CompletionAwardRewarded;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new CompletionRewardData();
        }

        public int GetCompletionRewardGuid(Instance instance)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardData.RewardGuid;
        }

        public Reward GetCompletionReward(Instance instance, RewardCatalogue rewardCatalogue)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardCatalogue.MustFindByGuid(rewardData.RewardGuid);
        }

        public bool HasCompletionRewardBeenAwarded(Instance instance)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardData.CompletionAwardRewarded;
        }

        public void AwardCompletionReward(Instance instance, RewardCatalogue rewardCatalogue, int rewardMultiplier)
        {
            UnityEngine.Debug.Assert(!HasCompletionRewardBeenAwarded(instance), $"Completion rewards have already been awarded.");
            Reward reward = GetCompletionReward(instance, rewardCatalogue);
            reward.AwardReward(rewardMultiplier);
        
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            rewardData.CompletionAwardRewarded = true;
            instance.events.ComponentDataChanged.Invoke();
        }
    }
}