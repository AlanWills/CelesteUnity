﻿using Celeste.Components;
using Celeste.Rewards.Catalogue;
using System;
using System.Collections.Generic;
using Celeste.Rewards.Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Celeste.LiveOps.Components
{
    [CreateAssetMenu(fileName = nameof(CompletionReward), menuName = "Celeste/Live Ops/Rewards/Completion Reward")]
    public class CompletionReward : Celeste.Components.Component, ILiveOpCompletionReward
    {
        #region Save Data

        [Serializable]
        public class CompletionRewardData : ComponentData
        {
            public int rewardGuid;
            public bool completionAwardRewarded;
        }

        #endregion

        public override ComponentData CreateData()
        {
            return new CompletionRewardData();
        }

        public int GetCompletionRewardGuid(Instance instance)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardData.rewardGuid;
        }

        public Reward GetCompletionReward(Instance instance, RewardCatalogue rewardCatalogue)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardCatalogue.MustFindByGuid(rewardData.rewardGuid);
        }

        public bool HasCompletionRewardBeenAwarded(Instance instance)
        {
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            return rewardData.completionAwardRewarded;
        }

        public void AwardCompletionReward(Instance instance, RewardCatalogue rewardCatalogue)
        {
            UnityEngine.Debug.Assert(!HasCompletionRewardBeenAwarded(instance), $"Completion rewards have already been awarded.");
            Reward reward = GetCompletionReward(instance, rewardCatalogue);
            reward.AwardReward();
        
            CompletionRewardData rewardData = instance.data as CompletionRewardData;
            rewardData.completionAwardRewarded = true;
            instance.events.ComponentDataChanged.Invoke();
        }
    }
}